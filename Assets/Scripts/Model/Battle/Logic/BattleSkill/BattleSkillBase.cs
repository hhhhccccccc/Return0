using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleSkillRepeatData
{
    public int TargetID { get; set; }
    public int SkillID { get; set; }
    public int VariantID { get; set; }
    public int MaxRepeatCount { get; set; }
    public bool IfLostChangeToOther { get; set; }
}

public class BattleSkillBase : BattleSkillMoment, IModel, IRecycle
{
    [Inject] protected ConfigManager ConfigManager { get; set; }
    [Inject] protected IPoolManager PoolManager { get; set; }
    [Inject] private BattleUtil BattleUtil { get; set; }
    [Inject] private BattleMomentConditionManager BattleMomentConditionManager { get; set; }
    public int SkillGuid { get; private set; }
    public int SkillID { get; private set; }
    public BattleUnit Subject { get; private set; }
    public BattleUnit Target { get; private set; }
    public BattleSkillConfig Config { get; private set; }
    /// <summary>
    /// 技能刚炁消耗
    /// </summary>
    private float GangQiCost { get; set; }
    public void SetGangQiCost(float gangQiCost) => GangQiCost = gangQiCost;
    public float GetGangQiCost() => GangQiCost;

    /// <summary>
    /// 技能玄炁消耗
    /// </summary>
    private float XuanQiCost { get; set; }
    public void SetXuanQiCost(float xuanQiCost) => XuanQiCost = xuanQiCost;
    public float GetXuanQiCost() => XuanQiCost;

    /// <summary>
    /// 技能的键消耗
    /// </summary>
    private List<int> KeyCostList { get; set; }
    public List<int> GetKeyCostList => KeyCostList;
    
    /// <summary>
    /// 技能威力
    /// </summary>
    private float SkillDamageRate { get; set; }
    public float GetSkillDamageRate => SkillDamageRate;
    public void SetSkillDamageRate(float damageRate) => SkillDamageRate = damageRate;
    
    /// <summary>
    /// 技能威力增伤倍率
    /// </summary>
    private float SkillWellyEffect { get; set; }
    public float GetSkillWellyEffect => SkillWellyEffect;
    public void SetSkillWellyEffect(float skillWellyEffect) => SkillWellyEffect = skillWellyEffect;
    
    /// <summary>
    /// 技能破防倍率
    /// </summary>
    private float SkillArmorPiercing { get; set; }
    public float GetSkillArmorPiercing => SkillArmorPiercing;
    public void SetSkillArmorPiercing(float skillArmorPiercing) => SkillArmorPiercing = skillArmorPiercing;

    /// <summary>
    /// 在行动期间是否被攻击过
    /// </summary>
    private bool BeDamageInSkillAction{ get; set; }
    public void SetBeDamageInSkillAction() => BeDamageInSkillAction = true;
    public bool GetBeDamageInSkillAction() => BeDamageInSkillAction;

    /// <summary>
    /// 技能类型
    /// </summary>
    private SkillType SkillType{ get; set; }
    public void SetSkillType(SkillType skillType) => SkillType = skillType;
    public SkillType GetSKillType => SkillType;

    /// <summary>
    /// 伤害类型
    /// </summary>
    private DamageType DamageType { get; set; }
    public void SetDamageType(DamageType damageType) => DamageType = damageType;
    public DamageType GetDamageType => DamageType;
    public List<int> GetRemoveMomentList => Config.SkillRemoveMoment;
    
    /// <summary>
    /// 判断技能期间经过了哪些阶段
    /// </summary>
    private HashSet<int> PassMomentList = new();
    private void AddPassMoment(BattleMomentType momentType)
    {
        PassMomentList.Add((int)momentType);
    }
    public bool CheckTriggerMoment(BattleMomentType momentType) => PassMomentList.Contains((int)momentType);

    public List<bool> ClashState = new();
    public void AddClashState(bool value) => ClashState.Add(value);
    
    /// <summary>
    /// 本次行动不会被破招 在息开始判断
    /// </summary>
    private int InActionDontBeCounter { get; set; }
    /// <summary>
    /// 本次交锋不会被破招
    /// </summary>
    private int InClashDontBeCounter { get; set; }
    /// <summary>
    /// 状态续存
    /// </summary>
    private bool InStatusPersists { get; set; }
    /// <summary>
    /// 增益状态续存
    /// </summary>
    private bool InGainStatusPersists { get; set; }
    /// <summary>
    /// 不受异常状态的影响
    /// </summary>
    private bool NotBeAbnormalBuffEffect { get; set; }
    
    /// <summary>
    /// 消耗的气
    /// </summary>
    private float TruthCostGangQi { get; set; }
    private float TruthCostXuanQi { get; set; }
    /// <summary>
    /// 消耗的键
    /// </summary>
    private List<int> TruthCostKey = new();
    /// <summary>
    /// 是否需要消耗
    /// </summary>
    public bool NeedCostResource { get; private set; }
    /// <summary>
    /// 是否是重复的招式
    /// </summary>
    public bool IsRepeat { get; private set; }
    /// <summary>
    /// 变式ID
    /// </summary>
    public int VariantID { get; set; }
    /// <summary>
    /// 实际的消耗数据
    /// </summary>
    public void SetTruthSkillCost(float gangQi, float xuanQi, List<int> keyCost)
    {
        TruthCostGangQi = gangQi;
        TruthCostXuanQi = xuanQi;
        TruthCostKey.ClearAndAddRange(keyCost);
    }
    public bool IsInAction { get; private set; }
    public virtual void Init(int skillGuid, BattleUnit subject, BattleUnit target, bool needCostResource = true, bool isRepeat = false)
    {
        SkillGuid = skillGuid;
        (SkillID, VariantID) = Util.UnCombSkillGuid(skillGuid);
        Config = ConfigManager.GetBattleSkillConfig(SkillID);
        Subject = subject;
        NeedCostResource = needCostResource;
        IsRepeat = isRepeat;
        SetTarget(target);
        BeDamageInSkillAction = false;
        InActionDontBeCounter = 0;
        InClashDontBeCounter = 0;
        PassMomentList.Clear();
        var preUseMgr = subject.PreUseSkillDataManager;
        var preGangQiCost = preUseMgr.GetSkillPreUseGangQiCost(SkillGuid);
        var preXuanQiCost = preUseMgr.GetSkillPreUseXuanQiCost(SkillGuid);
        foreach (var changeModel in subject.GetBattlePropertyChanged())
        {
            (preGangQiCost, preXuanQiCost) = changeModel.ChangeResourceCost(preGangQiCost, preXuanQiCost);
        }
        SetGangQiCost(preGangQiCost);
        SetXuanQiCost(preXuanQiCost);
        KeyCostList = preUseMgr.GetSkillPreUseKeyCost(SkillGuid);
        var damageRateBase = preUseMgr.GetSkillPreUseDamage(SkillGuid);
        SetSkillType(preUseMgr.GetSkillPreUseSkillType(SkillGuid));
        SetSkillDamageRate(damageRateBase);
        SetDamageType(preUseMgr.GetSkillPreUseDamageType(SkillGuid));
        SetSkillWellyEffect(preUseMgr.GetSkillWellyEffect(SkillGuid));
        SetSkillArmorPiercing(preUseMgr.GetSkillArmorPiercing(SkillGuid));
        InitMoment(this);
    }

    public bool SkillIsKillingStyle()
    {
        return BattleUtil.SkillIsKillingStyle(GetSKillType);
    }
   
    public void ReturnSkillResourceCost(bool returnGangQi = false, bool returnXuanQi = false, bool returnKey = false)
    { 
        if (returnGangQi)
        {
            Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, TruthCostGangQi);
        }
        
        if (returnXuanQi)
        {
            Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, TruthCostXuanQi);
        }
        
        if (returnKey)
        {
            var cost = GetKeyCostList;
            foreach (var key in cost)
            {
                Subject.ChangeKey((BattleKeyType)key, 1);
            }
        }
    }

    public BattlePropertyType GetFirstKeyType()
    {
        return (BattlePropertyType)GetKeyCostList[0];
    }

    public string GetSkillAniName() => Config.AniName;

    public void SetTarget(BattleUnit newTarget)
    {
        Target = newTarget;
    }
    /// <summary>
    /// 判断招式威力增长
    /// </summary>
    /// <param name="paramModel"></param>
    /// <returns></returns>
    protected virtual bool CheckSkillAddWellyDate(MomentParamModel paramModel)
    {
        if (Config.CheckSkillAddWellyRate.Count > 0)
        {
            if (Config.CheckSkillAddWellyRateRelation == 1 && Config.CheckSkillAddWellyRate.All(conditionID =>
                    BattleMomentConditionManager.GetCondition(conditionID, Subject, Target, SkillID, paramModel)))
            {
                return true;
            }

            if (Config.CheckSkillAddWellyRateRelation == 2 && Config.CheckSkillAddWellyRate.Any(conditionID =>
                    BattleMomentConditionManager.GetCondition(conditionID, Subject, Target, SkillID, paramModel)))
            {
                return true;
            }

            return false;
        }

        return true;
    }
    
    /// <summary>
    /// 重写招式威力增长
    /// </summary>
    /// <returns></returns>
    protected virtual float SkillAddWellyRate()
    {
        if (Config.SkillAddWellyRate.Count > 0)
        {
            return Config.SkillAddWellyRate[0];
        }

        return 0;
    }
    
    /// <summary>
    /// 获取招式威力增长
    /// </summary>
    /// <param name="paramModel"></param>
    /// <returns></returns>
    public float GetSkillAddWellyRate(MomentParamModel paramModel)
    {
        if (CheckSkillAddWellyDate(paramModel))
        {
            return SkillAddWellyRate();
        }

        return 0;
    }
    /// <summary>
    /// 判断招式伤害增长
    /// </summary>
    /// <param name="paramModel"></param>
    /// <returns></returns>
    protected virtual bool CheckSkillAddDamageRate(MomentParamModel paramModel)
    {
        if (Config.CheckSkillAddDamageRate.Count > 0)
        {
            if (Config.CheckSkillAddDamageRateRelation == 1 && Config.CheckSkillAddDamageRate.All(conditionID =>
                    BattleMomentConditionManager.GetCondition(conditionID, Subject, Target, SkillID, paramModel)))
            {
                return true;
            }

            if (Config.CheckSkillAddDamageRateRelation == 2 && Config.CheckSkillAddDamageRate.Any(conditionID =>
                    BattleMomentConditionManager.GetCondition(conditionID, Subject, Target, SkillID, paramModel)))
            {
                return true;
            }

            return false;
        }

        return true;
    }
    
    /// <summary>
    /// 重写招式伤害增长
    /// </summary>
    /// <returns></returns>
    protected virtual float SkillAddDamageRate() => Config.SkillAddDamageRate;
    
    /// <summary>
    /// 获取招式伤害增长
    /// </summary>
    /// <param name="paramModel"></param>
    /// <returns></returns>
    public float GetSkillAddDamageRate(MomentParamModel paramModel)
    {
        if (CheckSkillAddDamageRate(paramModel))
        {
            return SkillAddDamageRate();
        }

        return 0;
    }
    
    public override void SelfActionWheelStart()
    {
        base.SelfActionWheelStart();
        IsInAction = true;
        if (Config.ActionDontBeCounter > 0)
        {
            if ((Config.CheckActionDontBeCounter.Count <= 0)
                || (Config.CheckActionDontBeCounterRelation == 1 && Config.CheckActionDontBeCounter.All(conditionID => BattleMomentConditionManager.GetCondition(conditionID, Subject, Target, SkillID, null)))
                || (Config.CheckActionDontBeCounterRelation == 2 && Config.CheckActionDontBeCounter.Any(conditionID => BattleMomentConditionManager.GetCondition(conditionID, Subject, Target, SkillID, null))))
            {
                InActionDontBeCounter = Config.ActionDontBeCounter;
                SetSubjectDontBeCounter(InActionDontBeCounter, true);
            }
        }

        if (Config.StatusPersists > 0)
        {
            InStatusPersists = true;
            Subject.AddStatusPersists(1);
        }
        
        if (Config.GainStatusPersists > 0)
        {
            InGainStatusPersists = true;
            Subject.AddGainStatusPersists(1);
        }

        if (Config.NotBeAbnormalBuffEffect > 0)
        {
            NotBeAbnormalBuffEffect = true;
            Subject.AddNotBeAbnormalBuffEffect(1);
        }
    }

    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (Config.ClashDontBeCounter > 0)
        {
            if ((Config.CheckClashDontBeCounter.Count <= 0)
                || (Config.CheckClashDontBeCounterRelation == 1 && Config.CheckClashDontBeCounter.All(conditionID => BattleMomentConditionManager.GetCondition(conditionID, Subject, Target, SkillID, paramModel)))
                || (Config.CheckClashDontBeCounterRelation == 2 && Config.CheckClashDontBeCounter.Any(conditionID => BattleMomentConditionManager.GetCondition(conditionID, Subject, Target, SkillID, paramModel))))
            {
                InClashDontBeCounter = Config.ClashDontBeCounter;
                SetSubjectDontBeCounter(InClashDontBeCounter, true);
            }
        }
    }
    
    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        base.AfterUnderAction(paramModel);
        if (InClashDontBeCounter > 0)
        {
            SetSubjectDontBeCounter(InClashDontBeCounter, false);
            InClashDontBeCounter = 0;
        }
    }
    
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        if (InClashDontBeCounter > 0)
        {
            SetSubjectDontBeCounter(InClashDontBeCounter, false);
            InClashDontBeCounter = 0;
        }
        AddPassMoment(BattleMomentType.AfterAction);
    }

    
    
    /// <summary>
    /// 技能结束的时候调用技能结束扳机
    /// </summary>
    public void SkillEnd()
    {
        IsInAction = false;
        var subjectID = Subject.EntityID;
        foreach (var momentID in Config.SkillEndMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.SkillEnd));
        }

        if (InActionDontBeCounter > 0)
        {
            SetSubjectDontBeCounter(InActionDontBeCounter, false);
            InActionDontBeCounter = 0;
        }
        
        if (InStatusPersists)
        {
            InStatusPersists = false;
            Subject.AddStatusPersists(-1);
        }
        
        if (InGainStatusPersists)
        {
            InGainStatusPersists = false;
            Subject.AddGainStatusPersists(-1);
        }

        if (NotBeAbnormalBuffEffect)
        {
            NotBeAbnormalBuffEffect = false;
            Subject.AddNotBeAbnormalBuffEffect(-1);
        }
    }

    private void SetSubjectDontBeCounter(int typeID, bool add)
    {
        var state = add ? 1 : -1;
        switch (typeID)
        {
            case 1:
                Subject.SetDontBeCounter(state);
                break;
            case 2:
                Subject.AddIgnoreTargetNotHasUpBuff(state);
                break;
            case 3:
                Subject.AddIgnoreTargetNotHasDownBuff(state);
                break;
            case 4:
                Subject.AddIgnoreTargetNotHasLeftBuff(state);
                break;
            case 5:
                Subject.AddIgnoreTargetNotHasRightBuff(state);
                break;
            case 6:
                Subject.SetDontBeCounterByPowerKilling(state);
                break;
            case 7:
                Subject.SetDontBeCounterByArtKilling(state);
                break;
            case 8:
                Subject.AddIgnoreTargetSkillNotHasUpKey(state);
                break;
            case 9:
                Subject.AddIgnoreTargetSkillNotHasDownKey(state);
                break;
            case 10:
                Subject.AddIgnoreTargetSkillNotHasLeftKey(state);
                break;
            case 11:
                Subject.AddIgnoreTargetSkillNotHasRightKey(state);
                break;
        }
    }

    public virtual bool IsTrueDamage(DamageParamModel model) => false;
    
    public virtual void Recycle()
    {
        VariantID = 0;
        SkillID = 0;
        Subject = null;
        Target = null;
        GangQiCost = 0;
        XuanQiCost = 0;
        KeyCostList.Clear();
        SkillDamageRate = 0;
        SkillWellyEffect = 0;
        SkillArmorPiercing = 0;
        BeDamageInSkillAction = false;
        SkillType = SkillType.None;
        DamageType = DamageType.None;
        InActionDontBeCounter = 0;
        InClashDontBeCounter = 0;
        InStatusPersists = false;
        InGainStatusPersists = false;
        NotBeAbnormalBuffEffect = false;
        ClashState.Clear();
        TruthCostKey.Clear();
        TruthCostGangQi = 0;
        TruthCostXuanQi = 0;
        NeedCostResource = false;
    }

    public virtual BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null) => null;

    public float GetProperty(BattlePropertyType propType)
    {
        if (IsInAction)
        {
            #region 心法10060转化效果

            var hasMethod10060 = Subject.CheckHasMethod(GameConst.Battle.HeartMethod10060);
            if (hasMethod10060)
            {
                if (propType == BattlePropertyType.BreakPct || propType == BattlePropertyType.DefendPct)
                {
                    return 0;
                }

                if (propType == BattlePropertyType.TechPct || propType == BattlePropertyType.PowerPct)
                {
                    return Config.BreakDefendAddRate;
                }
            }

            #endregion
            
            if (propType == BattlePropertyType.BreakPct || propType == BattlePropertyType.DefendPct)
            {
                return Config.BreakDefendAddRate;
            }
        }

        return 0;
    }
}