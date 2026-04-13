using System;
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

public class BattleSkillBase : BattleMoment
{
    [Inject] protected BattleTypeManager BattleTypeManager { get; set; }
    public int SkillGuid { get; private set; }
    public int SkillID { get; private set; }
    public BattleUnit Target { get; private set; }
    public BattleSkillConfig Config { get; private set; }
    protected override int GetSymbol => 100000 + Config.Id;
    protected override float GetConfigParamFloat(int index)
    {
        return Config.ParamEx[index];
    }

    public override int GetConfigParamInt(int index)
    {
        return Config.ParamEx[index].ToInt();
    }

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
    private List<int> KeyCostList { get; set; } = new();
    public List<int> GetKeyCostList => KeyCostList;
    
    /// <summary>
    /// 技能基础威力
    /// </summary>
    private float WellyRateBase { get; set; }
    public float GetWellyRateBase(MomentParamModel paramModel) => WellyRateBase;
    private void SetWellyRateBase(float wellyRateBase) => WellyRateBase = wellyRateBase;
    
    /// <summary>
    /// 技能威力增伤倍率
    /// </summary>
    private float WellyEffect { get; set; }
    public override float GetWellyIncrease(int skillGuid)
    {
        return WellyEffect;
    }
    private void SetWellyEffect(float wellyEffect) => WellyEffect = wellyEffect;

    /// <summary>
    /// 技能破防倍率
    /// </summary>
    private float SkillArmorPiercing { get; set; }
    public float GetSkillArmorPiercing => SkillArmorPiercing;
    public void SetSkillArmorPiercing(float skillArmorPiercing) => SkillArmorPiercing = skillArmorPiercing;

    /// <summary>
    /// 在行动期间是否被直接攻击过
    /// </summary>
    public bool BeDirectDamageInSkillAction{ get; private set; }

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
    public List<BattleKey> TruthCostKey = new();
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
    public void SetTruthSkillCost(float gangQi, float xuanQi, List<BattleKey> keyCost)
    {
        TruthCostGangQi = gangQi;
        TruthCostXuanQi = xuanQi;
        TruthCostKey.ClearAndAddRange(keyCost);
    }
    public bool IsInAction { get; private set; }
    public BattleVariantBase Variant { get; set; }
    public virtual void Init(int skillGuid, BattleUnit subject, BattleUnit target, bool needCostResource = true, bool isRepeat = false)
    {
        SkillGuid = skillGuid;
        (SkillID, VariantID) = Util.UnCombSkillGuid(skillGuid);
        Config = ConfigManager.GetBattleSkillConfig(SkillID);
        Subject = subject;
        NeedCostResource = needCostResource;
        IsRepeat = isRepeat;
        SetTarget(target);
        BeDirectDamageInSkillAction = false;
        InActionDontBeCounter = 0;
        InClashDontBeCounter = 0;
        PassMomentList.Clear();
        var preUseMgr = subject.PreUseSkillDataManager;
        var preGangQiCost = preUseMgr.GetSkillPreUseGangQiCost(SkillGuid);
        var preXuanQiCost = preUseMgr.GetSkillPreUseXuanQiCost(SkillGuid);
        (preGangQiCost, preXuanQiCost) = subject.BattleMomentManager.ChangeResourceCost(preGangQiCost, preXuanQiCost);
        SetGangQiCost(preGangQiCost);
        SetXuanQiCost(preXuanQiCost);
        KeyCostList.ClearAndAddRange(preUseMgr.GetSkillPreUseKeyCost(SkillGuid));
        var wellyRateBase = preUseMgr.GetSkillPreUseWellyRateBase(SkillGuid);
        SetSkillType(preUseMgr.GetSkillPreUseSkillType(SkillGuid));
        SetWellyRateBase(wellyRateBase);
        SetDamageType(preUseMgr.GetSkillPreUseDamageType(SkillGuid));
        SetWellyEffect(preUseMgr.GetSkillWellyEffect(SkillGuid));
        SetSkillArmorPiercing(preUseMgr.GetSkillArmorPiercing(SkillGuid));
        InitVariant();
    }

    private void InitVariant()
    {
        if (VariantID > 0)
        {
            Variant = (BattleVariantBase)PM.GetClass(BattleTypeManager.GetSkillType(VariantID));
            Variant.Init(SkillGuid, Subject, Target, this);
        }
    }

    public override BattleMomentViewModel AllocViewModel(int entityID, MomentViewType viewType)
    {
        var viewModel = PM.GetClass<BattleMomentViewModel>();
        viewModel.BattleSource = BattleSource.Skill;
        viewModel.EntityID = entityID;
        viewModel.ConfigID = SkillGuid;
        return viewModel;
    }

    public bool SkillIsKillingStyle()
    {
        return BattleUtil.SkillIsKillingStyle(GetSKillType);
    }

    public string GetSkillAniName() => Config.AniName;

    public void SetTarget(BattleUnit newTarget)
    {
        Target = newTarget;
    }
  
    public override void SelfActionWheelStart()
    {
        IsInAction = true;
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
        
        OnSelfActionWheelStart();
    }

    protected virtual void OnSelfActionWheelStart()
    {
        
    }
    
    public override void AfterAction(MomentParamModel paramModel)
    {
        AddPassMoment(BattleMomentType.AfterAction);
        OnAfterAction(paramModel);
    }

    protected virtual void OnAfterAction(MomentParamModel paramModel)
    {
        
    }

    public override void SkillEnd(BattleSkillBase skill)
    {
        IsInAction = false;
        
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
        
        OnSkillEnd(skill);
    }

    protected virtual void OnSkillEnd(BattleSkillBase skill)
    {
        
    }
    
    protected virtual int DontBeCounterState(MomentParamModel paramModel) => 0;
    public override bool CheckDontBeCounter(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (!IsInAction)
            {
                return false;
            }
            
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherSkillType = model.GetOtherSkillType(Subject.EntityID);
            var otherCostKey = model.GetOtherKeyCost(Subject.EntityID);
            var dontBeCounterType = DontBeCounterState(paramModel);
            switch (dontBeCounterType)
            {
                case 1:
                    return true;
                case 2:
                    return !BattleBuffManager.CheckTargetHasUpSkillBuff(otherID);
                case 3:
                    return !BattleBuffManager.CheckTargetHasDownSkillBuff(otherID);
                case 4:
                    return !BattleBuffManager.CheckTargetHasLeftSkillBuff(otherID);
                case 5:
                    return !BattleBuffManager.CheckTargetHasRightSkillBuff(otherID);
                case 6:
                    return otherSkillType == SkillType.PowerKilling;
                case 7:
                    return otherSkillType == SkillType.ArtKilling;
                case 8:
                    return otherCostKey.All(key => key.KeyType != BattleKeyType.KeyUp);
                case 9:
                    return otherCostKey.All(key => key.KeyType != BattleKeyType.KeyDown);
                case 10:
                    return otherCostKey.All(key => key.KeyType != BattleKeyType.KeyLeft);
                case 11: 
                    return otherCostKey.All(key => key.KeyType != BattleKeyType.KeyRight);
            }
        }
        
        return false;
    }

    public virtual bool IsTrueDamage(DamageParamModel model) => false;
    
    public virtual BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null) => null;

    public override float GetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (IsInAction)
        {
            #region 心法10060转化效果

            var hasMethod10060 = Subject.BattleMomentManager.CheckHasMethod(GameConst.Battle.HeartMethod10060);
            if (hasMethod10060)
            {
                if (propertyType == BattlePropertyType.BreakPct || propertyType == BattlePropertyType.DefendPct)
                {
                    return 0;
                }

                if (propertyType == BattlePropertyType.TechPct || propertyType == BattlePropertyType.PowerPct)
                {
                    return Config.BreakDefendAddRate;
                }
            }
            
            #endregion
            
            if (propertyType == BattlePropertyType.BreakPct || propertyType == BattlePropertyType.DefendPct)
            {
                return Config.BreakDefendAddRate;
            }
        }

        return 0;
    }

    public override void BeDamage(DamageType damageType)
    {
        if (IsInAction && damageType == DamageType.Direct)
        {
            BeDirectDamageInSkillAction = true;
        }
        OnBeDamage(damageType);
    }

    protected virtual void OnBeDamage(DamageType damageType)
    {
        
    }

    protected override void OnRecycle()
    {
        VariantID = 0;
        SkillID = 0;
        Subject = null;
        Target = null;
        GangQiCost = 0;
        XuanQiCost = 0;
        KeyCostList.Clear();
        WellyRateBase = 0;
        WellyEffect = 0;
        SkillArmorPiercing = 0;
        BeDirectDamageInSkillAction = false;
        SkillType = SkillType.None;
        DamageType = DamageType.None;
        InActionDontBeCounter = 0;
        InClashDontBeCounter = 0;
        InStatusPersists = false;
        InGainStatusPersists = false;
        NotBeAbnormalBuffEffect = false;
        ClashState.Clear();
        TruthCostGangQi = 0;
        TruthCostXuanQi = 0;
        TruthCostKey.Clear();
        NeedCostResource = false;
        PM.RecycleClass(Variant);
        Variant = null;
        OnSkillRecycle();
    }
    
    protected virtual void OnSkillRecycle() {}
    
    #region 常用Effect执行方法

    
    #endregion
}