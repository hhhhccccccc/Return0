using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuffBase : BattleBuffMoment, IModel
{
    [Inject] private ConfigManager ConfigManager;
    [Inject] private BattleMomentConditionManager BattleMomentConditionManager;
    [Inject] private BattleMomentManager BattleMomentManager;
    [Inject] private BattleBuffManager BattleBuffManager;
    private int BuffID;

    public BattleBuffConfig Config;
    public BattleUnit SpellCaster;
    public BattleUnit Subject;

    public int LayerCount;
    private int LastGetSumCount;//最后一次获得buff是的总数
    protected List<float> ParamList;
    private int Limit;
    
    public virtual void AddToUnit(int buffID, BattleUnit subject, BattleUnit spellCaster, int addCount, List<float> paramList = null)
    {
        BuffID = buffID;
        Config = ConfigManager.GetBattleBuffConfig(BuffID);
        Subject = subject;
        SpellCaster = spellCaster;
        ParamList = paramList;
        Limit = Config.Limit;
        InitMoment(this);
        AddLayerCount(addCount);
        Start();
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
        for (int i = 1; i <= layerCount; i++)
        {
            if (layerCount < Limit || Limit == -1)
            {
                TriggerBuffAdd();
                LayerCount++;
            }
        }
    }
    
    protected virtual void OnStart()
    {
        
    }

    private void TriggerBuffAdd()
    {
        var subjectID = Subject.EntityID;
        var spellCasterID = SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Config.BuffAddMoment)
        {
            EnqueueViewModel(BattleMomentType.BuffAdd, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null));
        }
    }

    private void TriggerBuffReduce()
    {
        var subjectID = Subject.EntityID;
        var spellcasterID = SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Config.BuffReduceMoment)
        {
            EnqueueViewModel(BattleMomentType.BuffReduce, BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID, null));
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
            case BuffReduceType.BeHit:
                if (paramModel is DamageParamModel damageParamModel)
                {
                    
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
        for (int i = 1; i <= layerCount; i++)
        {
            TriggerBuffReduce();
            LayerCount--;
        }

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
}
