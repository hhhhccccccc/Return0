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
        InitMoment(this);
        AddLayerCount(addCount);
        Start();
        LogManager.Debug($"{subject.EntityID}得到buffID : {buffID}, 施法者 : {SpellCaster?.EntityID ?? 0}");
    }
    
    private void Start()
    {
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
            TriggerBuffAdd(layerCount);
            LayerCount += layerCount;
        }
        else
        {
            layerCount = Math.Min(Limit - LayerCount, layerCount);
            TriggerBuffAdd(layerCount);
            LayerCount += layerCount;
        }
    }
    
    public bool IsMaxLayer() => LayerCount == Config.Limit;
    
    protected virtual void OnStart()
    {
        
    }

    private void TriggerBuffAdd(int addLayerCount)
    {
        var subjectID = Subject.EntityID;
        var spellCasterID = SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Config.BuffAddMoment)
        {
            EnqueueViewModel(BattleMomentType.BuffAdd, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, addLayerCount));
        }
    }

    private void TriggerBuffReduce(int reduceLayerCount)
    {
        var subjectID = Subject.EntityID;
        var spellCasterID = SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Config.BuffReduceMoment)
        {
            EnqueueViewModel(BattleMomentType.BuffReduce, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, reduceLayerCount));
        }
    }

    public virtual bool CheckSkillCanUse(int skillID)
    {
        if (Config.CheckSkillRelease.Count <= 0)
            return true;

        return Config.CheckSkillReleaseRelation switch
        {
            1 => Config.CheckSkillRelease.All(conditionID =>
                BattleMomentConditionManager.GetCondition(conditionID, Subject, skillID, null)),
            2 => Config.CheckSkillRelease.Any(conditionID =>
                BattleMomentConditionManager.GetCondition(conditionID, Subject, skillID, null)),
            _ => false
        };
    }

    public void ReduceLayer(BuffReduceType reduceType, MomentParamModel paramModel = null)
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
        layerCount = Math.Min(layerCount, LayerCount);
        LayerCount -= layerCount;
        TriggerBuffReduce(layerCount);
        if (LayerCount <= 0)
        {
            Subject.RemoveBuff(BuffID);
        }
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
        LayerCount = 0;
        LastGetSumCount = 0;
        BeforeLastActionGetLayerCount = 0;
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        BeforeLastActionGetLayerCount = LayerCount;
        base.AfterAction(paramModel);
    }
}
