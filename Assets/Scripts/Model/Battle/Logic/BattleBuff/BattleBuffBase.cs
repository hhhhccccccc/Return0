using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuffBase : BattleBuffMoment, IModel, IRecycle
{
    [Inject] private ConfigManager ConfigManager { get; set; }
    [Inject] private BattleMomentConditionManager BattleMomentConditionManager { get; set; }
    [Inject] private BattleMomentManager BattleMomentManager { get; set; }
    [Inject] protected BattleBuffManager BattleBuffManager { get; set; }
    [Inject] private ILogManager LogManager { get; set; }
    public int BuffID { get; private set; }
    public BuffType BuffType { get; private set; }
    public BattleBuffConfig Config { get; private set; }
    public BattleUnit SpellCaster { get; private set; }
    public BattleUnit Subject { get; private set; }
    public int LayerCount { get; private set; }
    private int LastGetSumCount { get; set; } //最后一次获得buff是的总数
    public List<float> ParamList { get; } = new();
    private int Limit { get; set; }
    private int BeforeLastActionGetLayerCount { get; set; }//最后一次行动前获取的全部
    
    public virtual void AddToUnit(int buffID, BattleUnit subject, BattleUnit spellCaster, int addCount, List<float> paramList = null)
    {
        BuffID = buffID;
        Config = ConfigManager.GetBattleBuffConfig(BuffID);
        BuffType = (BuffType)Config.BuffType;
        Subject = subject;
        SpellCaster = spellCaster;
        ParamList.Clear();
        if (paramList != null)
        {
            ParamList.AddRange(paramList);
        }
        Limit = Config.Limit;
        InitMoment(this, ReduceLayer);
        AddLayerCount(addCount);
        Start();
        LogManager.Debug($"{subject.EntityID}得到buffID : {buffID}, 施法者 : {SpellCaster?.EntityID ?? 0}");
    }
    
    private void Start()
    {
        var subjectID = Subject.EntityID;
        var spellCasterID = SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Config.BuffStartMoment)
        {
            EnqueueViewModel(BattleMomentType.BuffStart, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, 0));
        }
        OnStart();
        if (Config.OverlayType == (int)BuffOverlayType.Dispose)
        {
            ClearLayerCount();
        }
    }
    
    public virtual void AddLayerCount(int layerCount)
    {
        if (Limit == -1)
        {
            TriggerBuffAddLayerMoment(layerCount);
            LayerCount += layerCount;
        }
        else
        {
            layerCount = Math.Min(Limit - LayerCount, layerCount);
            TriggerBuffAddLayerMoment(layerCount);
            LayerCount += layerCount;
        }
    }
    
    public bool IsMaxLayer() => LayerCount == Config.Limit;
    
    protected virtual void OnStart()
    {
        
    }

    private void TriggerBuffAddLayerMoment(int addLayerCount)
    {
        var subjectID = Subject.EntityID;
        var spellCasterID = SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Config.BuffAddLayerMoment)
        {
            EnqueueViewModel(BattleMomentType.BuffAddLayer, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, addLayerCount));
        }
    }

    private void TriggerBuffReduceLayerMoment(int reduceLayerCount)
    {
        var subjectID = Subject.EntityID;
        var spellCasterID = SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Config.BuffReduceMoment)
        {
            EnqueueViewModel(BattleMomentType.BuffReduceLayer, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, reduceLayerCount));
        }
    }

    public virtual bool CheckSkillCanUse(int skillID)
    {
        if (Config.DoDesitionMoment.Count <= 0)
            return true;

        return Config.CheckSkillDoDesitionRelation switch
        {
            1 => Config.DoDesitionMoment.All(conditionID =>
                BattleMomentConditionManager.GetCondition(conditionID, Subject, skillID, null)),
            2 => Config.DoDesitionMoment.Any(conditionID =>
                BattleMomentConditionManager.GetCondition(conditionID, Subject, skillID, null)),
            _ => false
        };
    }
    /// <summary>
    /// buff层数减少机制
    /// </summary>
    /// <param name="reduceType"></param>
    /// <param name="paramModel"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    private void ReduceLayer(BuffReduceType reduceType, MomentParamModel paramModel = null)
    {
        int reduceCount = 0;
        
        switch (reduceType)
        {
            case BuffReduceType.None:
                return;
            case BuffReduceType.One:
                reduceCount = 1;
                break;
            case BuffReduceType.Clear:
                reduceCount = LayerCount;
                break;
            case BuffReduceType.AllCountPct1Q4:
                reduceCount = Math.Max(LastGetSumCount / 4, 1);
                break;
            case BuffReduceType.AllCountPct1Q3:
                reduceCount = Math.Max(LastGetSumCount / 3, 1);
                break;
            case BuffReduceType.AllCountPct1Q2:
                reduceCount = Math.Max(LastGetSumCount / 2, 1);
                break;
            case BuffReduceType.BeforeLastActionGetAll:
                if (BeforeLastActionGetLayerCount > 0)
                {
                    reduceCount = BeforeLastActionGetLayerCount;
                    BeforeLastActionGetLayerCount = 0;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(reduceType), reduceType, null);
        }
        
        ReduceLayerCount(reduceCount);
    }
    
    public void ReduceLayerCount(int layerCount)
    {
        if (IgnoreReduceLayer)
        {
            return;
        }
        
        layerCount = Math.Min(layerCount, LayerCount - BuffNotLowerLayerCount);
        layerCount = Math.Max(layerCount, 0);
        LayerCount -= layerCount;
        TriggerBuffReduceLayerMoment(layerCount);
        if (LayerCount <= 0)
        {
            var subjectID = Subject.EntityID;
            var spellCasterID = SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Config.BuffRemoveMoment)
            {
                EnqueueViewModel(BattleMomentType.BuffRemove, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, 0));
            }
            OnBuffRemove();
            Subject.RemoveBuff(BuffID);
        }
    }

    protected virtual void OnBuffRemove()
    {
        
    }
    
    public void ClearLayerCount()
    {
        ReduceLayerCount(LayerCount);
    }

    public virtual float GetShield() => 0;
    /// <summary>
    /// 返回扣除了多少的盾, ref 还剩下多少的伤害
    /// </summary>
    /// <param name="allDamage"></param>
    /// <returns></returns>
    public virtual float ReduceShield(ref float allDamage) => 0;

    public virtual float GetArmor() => 0;

    public virtual float ReduceArmor(ref float allDamage) => 0;

    public virtual void Recycle()
    {
        BuffID = 0;
        BuffType = BuffType.None;
        Config = null;
        SpellCaster = null;
        Subject = null;
        LayerCount = 0;
        ParamList.Clear();
        Limit = 0;
        LastGetSumCount = 0;
        BeforeLastActionGetLayerCount = 0;
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        BeforeLastActionGetLayerCount = LayerCount;
        base.AfterAction(paramModel);
    }

    /// <summary>
    /// buff不会减少层数
    /// </summary>
    private bool IgnoreReduceLayer;
    public void SetIgnoreReduceLayer(bool isIgnore) => IgnoreReduceLayer = isIgnore;

    /// <summary>
    /// buff不会低于几层
    /// </summary>
    private int BuffNotLowerLayerCount;
    public void SetBuffNotLowerLayerCount(int layerCount) => BuffNotLowerLayerCount += layerCount;
}
