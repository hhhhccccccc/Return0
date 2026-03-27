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

public class BattleSkillBase : BattleSkillMoment, IModel, IRecycle
{
    [Inject] protected ConfigManager ConfigManager { get; set; }
    [Inject] protected BattleManager BattleManager { get; set; }
    [Inject] protected BattleBuffManager BattleBuffManager { get; set; }
    [Inject] protected IPoolManager PoolManager { get; set; }
    [Inject] private BattleUtil BattleUtil { get; set; }
    [Inject] private BattleMomentConditionManager BattleMomentConditionManager { get; set; }
    [Inject] protected BattleLogicStateManager BattleLogicStateManager { get; set; }
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
    private List<int> KeyCostList { get; set; } = new();
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
    private List<BattleKey> TruthCostKey = new();
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
        (preGangQiCost, preXuanQiCost) = subject.BattleChangeModelManager.ChangeResourceCost(preGangQiCost, preXuanQiCost);
        SetGangQiCost(preGangQiCost);
        SetXuanQiCost(preXuanQiCost);
        KeyCostList.ClearAndAddRange(preUseMgr.GetSkillPreUseKeyCost(SkillGuid));
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
            Subject.ChangeKeyList(TruthCostKey.Select(costKey => costKey.KeyType).ToList(), true, ChangeKeyReason.SkillEffect);
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
    
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        AddPassMoment(BattleMomentType.AfterAction);
    }
    
    /// <summary>
    /// 技能结束的时候调用技能结束扳机
    /// </summary>
    public virtual void SkillEnd()
    {
        IsInAction = false;
        var subjectID = Subject.EntityID;
        foreach (var momentID in Config.SkillEndMoment)
        {
            BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.SkillEnd);
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

    public bool CheckDontBeCounter(MomentParamModel paramModel)
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
            
            if ((Config.ActionDontBeCounter > 0) && (Config.CheckActionDontBeCounter.Count <= 0)
                || (Config.CheckActionDontBeCounterRelation == 1 && Config.CheckActionDontBeCounter.All(conditionID => BattleMomentConditionManager.GetCondition(conditionID, Subject, Target, SkillID, null)))
                || (Config.CheckActionDontBeCounterRelation == 2 && Config.CheckActionDontBeCounter.Any(conditionID => BattleMomentConditionManager.GetCondition(conditionID, Subject, Target, SkillID, null))))
            {
                switch (Config.ActionDontBeCounter)
                {
                    case 1:
                        return true;
                    case 2:
                        return !BattleBuffManager.CheckTargetHasUpFirstSkillBuff(otherID);
                    case 3:
                        return !BattleBuffManager.CheckTargetHasDownFirstSkillBuff(otherID);
                    case 4:
                        return !BattleBuffManager.CheckTargetHasLeftFirstSkillBuff(otherID);
                    case 5:
                        return !BattleBuffManager.CheckTargetHasRightFirstSkillBuff(otherID);
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
            
            if ((Config.ClashDontBeCounter > 0) && (Config.CheckClashDontBeCounter.Count <= 0)
                || (Config.CheckClashDontBeCounterRelation == 1 && Config.CheckClashDontBeCounter.All(conditionID => BattleMomentConditionManager.GetCondition(conditionID, Subject, Target, SkillID, paramModel)))
                || (Config.CheckClashDontBeCounterRelation == 2 && Config.CheckClashDontBeCounter.Any(conditionID => BattleMomentConditionManager.GetCondition(conditionID, Subject, Target, SkillID, paramModel))))
            {
                switch (Config.ActionDontBeCounter)
                {
                    case 1:
                        return true;
                    case 2:
                        return !BattleBuffManager.CheckTargetHasUpFirstSkillBuff(otherID);
                    case 3:
                        return !BattleBuffManager.CheckTargetHasDownFirstSkillBuff(otherID);
                    case 4:
                        return !BattleBuffManager.CheckTargetHasLeftFirstSkillBuff(otherID);
                    case 5:
                        return !BattleBuffManager.CheckTargetHasRightFirstSkillBuff(otherID);
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
        }
        
        return false;
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
        TruthCostGangQi = 0;
        TruthCostXuanQi = 0;
        TruthCostKey.Clear();
        NeedCostResource = false;
    }

    public virtual BattleSkillRepeatData GetRepeatData(DamageParamModel paramModel = null) => null;

    public float GetProperty(BattlePropertyType propType)
    {
        if (IsInAction)
        {
            #region 心法10060转化效果

            var hasMethod10060 = Subject.BattleChangeModelManager.CheckHasMethod(GameConst.Battle.HeartMethod10060);
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
    
    /// <summary>
    /// 检查自己技能是否经过特定时机
    /// </summary>
    /// <param name="momentType">时机类型</param>
    /// <returns></returns>
    protected bool CheckSkillTriggerMoment(BattleMomentType momentType)
    {
        return Subject?.GetSkill()?.CheckTriggerMoment(momentType) == true;
    }

    /// <summary>
    /// 随机转化所有键
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="addCount">额外增加的键数量</param>
    protected void DoRandomAllKey(BattleUnit target, int addCount = 0)
    {
        if (target == null) return;
        target.RemoveAllKey(ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
        var count = target.GetAllKeyCount() + addCount;
        var list = Util.GetRandomKey(count);
        Subject.ChangeKeyList(list, true, ChangeKeyReason.SkillEffect);
    }

    /// <summary>
    /// 添加行动次数
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="times">次数</param>
    protected void DoAddActionTimes(BattleUnit target, int times = 1)
    {
        if (target == null) return;
        target.AddActionTimes(times);
    }

    /// <summary>
    /// 移除所有键并添加各种键
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="count">每种键的数量</param>
    protected void DoRemoveAllKeyAndAddAllKey(BattleUnit target, int count = 1)
    {
        if (target == null) return;
        var list = new List<BattleKeyType>();
        for (int i = 1; i <= count; i++)
        {
            list.Add(BattleKeyType.KeyUp);
            list.Add(BattleKeyType.KeyDown);
            list.Add(BattleKeyType.KeyLeft);
            list.Add(BattleKeyType.KeyRight);
        }
        target.RemoveAllKey(ChangeKeyReason.SkillEffect, ChangeKeyType.Convert);
        Subject.ChangeKeyList(list, true, ChangeKeyReason.SkillEffect, ChangeKeyType.Convert);
    }

    /// <summary>
    /// 改变属性（支持百分比和绝对值）
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="propertyType">属性类型</param>
    /// <param name="value">值（绝对值或百分比）</param>
    /// <param name="source">来源</param>
    protected void DoChangeProperty(BattleUnit target, BattlePropertyType propertyType, float value, BattleSource source = BattleSource.None)
    {
        if (target == null) return;
        target.ChangeProperty(propertyType, value, source);
    }

    /// <summary>
    /// 转换伤害为甲
    /// </summary>
    /// <param name="target">目标单位</param>
    protected void DoConvertDamageToArmorBuff(BattleUnit target)
    {
        if (target == null) return;
        // 需要根据战斗中的伤害量来添加甲
        // 这个效果通常需要在战斗过程中触发，这里先预留
        // TODO: 需要确认具体实现方式
    }

  

    #region 常用Effect执行方法

    /// <summary>
    /// 设置目标到当前息
    /// </summary>
    /// <param name="target">目标单位</param>
    protected void DoSetActionWheelToNow(BattleUnit target)
    {
        if (target == null) return;
        target.SetActionWheelToNow();
    }

    /// <summary>
    /// 添加Buff
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="buffID">BuffID</param>
    /// <param name="spellCaster">施法者</param>
    /// <param name="layerCount">层数</param>
    /// <param name="paramList">参数</param>
    /// <param name="momentType">时机类型</param>
    protected void DoAddBuff(BattleUnit target, int buffID, BattleUnit spellCaster, int layerCount, List<float> paramList, BattleMomentType momentType)
    {
        if (target == null) return;
        BattleBuffManager.AddBuff(target, buffID, spellCaster ?? Subject, layerCount, paramList, momentType);
    }

    /// <summary>
    /// 添加随机键
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="count">数量</param>
    /// <param name="reason">原因</param>
    protected void DoAddRandomKey(BattleUnit target, int count, ChangeKeyReason reason)
    {
        if (target == null) return;
        target.AddRandomKey(count, reason);
    }

    /// <summary>
    /// 恢复属性（刚气/玄气）
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="propertyType">属性类型</param>
    /// <param name="value">恢复值</param>
    protected void DoChangeProperty(BattleUnit target, BattlePropertyType propertyType, int value)
    {
        if (target == null) return;
        target.ChangeProperty_Abs(propertyType, value);
    }

    /// <summary>
    /// 加快息
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="value">加快值</param>
    protected void DoChangeActionWheel(BattleUnit target, int value)
    {
        if (target == null) return;
        target.ChangeActionWheel(value);
    }

    /// <summary>
    /// 获取护体Buff
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="pct">力量百分比</param>
    /// <param name="momentType">时机类型</param>
    protected void DoGetShieldBuff(BattleUnit target, float pct, BattleMomentType momentType)
    {
        if (target == null) return;
        var power = target.GetProperty(BattlePropertyType.Power);
        BattleBuffManager.AddBuff(target, GameConst.Battle.ShieldBuffID, target, (power * pct).ToInt(), null, momentType);
    }

    /// <summary>
    /// 获取甲Buff
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="pct">力量百分比</param>
    /// <param name="momentType">时机类型</param>
    protected void DoGetArmorBuff(BattleUnit target, float pct, BattleMomentType momentType)
    {
        if (target == null) return;
        var power = target.GetProperty(BattlePropertyType.Power);
        BattleBuffManager.AddBuff(target, GameConst.Battle.ArmorBuffID, target, (power * pct).ToInt(), null, momentType);
    }

    /// <summary>
    /// 设置技能刚炁消耗
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="pct">当前刚炁百分比</param>
    /// <param name="maxCost">最大消耗上限</param>
    protected void DoChangeSkillGangQiCost(BattleUnit target, float pct, float maxCost)
    {
        if (target == null) return;
        var skill = target.GetSkill();
        if (skill == null) return;
        var curr = target.GetProperty(BattlePropertyType.GangQi);
        var cost = curr * pct;
        if (maxCost > 0)
        {
            cost = Math.Min(cost, maxCost);
        }
        skill.SetGangQiCost(cost);
    }

    /// <summary>
    /// 移除指定Buff
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="buffID">BuffID</param>
    protected void DoRemoveBuff(BattleUnit target, int buffID)
    {
        if (target == null) return;
        var buffs = target.GetBuffList();
        foreach (var buff in buffs)
        {
            if (buff.BuffID == buffID)
            {
                target.ClearBuff(buffID);
                break;
            }
        }
    }

    /// <summary>
    /// 设置技能期间被打了
    /// </summary>
    /// <param name="target">目标单位</param>
    protected void DoSetBeDamageInSkillAction(BattleUnit target)
    {
        if (target == null) return;
        var skill = target.GetSkill();
        if (skill != null)
        {
            skill.SetBeDamageInSkillAction();
        }
    }

    /// <summary>
    /// 检查自己技能期间是否被打了（条件100001）
    /// </summary>
    /// <returns></returns>
    protected bool CheckBeDamageInSkillAction()
    {
        return Subject != null && Subject.GetSkill()?.GetBeDamageInSkillAction() == true;
    }

    /// <summary>
    /// 转换异常Buff为增益Buff
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="poolID">增益Buff池ID</param>
    /// <param name="convertCount">转换数量</param>
    protected void DoConvertBuffAbnormalToGain(BattleUnit target, int poolID, int convertCount = 1)
    {
        if (target == null) return;
        // 获取随机异常Buff
        var clearBuffList = target.GetRandomBuffByType(BuffType.Abnormal, convertCount);
        var clearCount = clearBuffList.Count;
        foreach (var buff in clearBuffList)
        {
            target.ClearBuff(buff.BuffID);
        }
        // 从增益池获取Buff并添加需要ConfigHelper，暂不完整实现
    }

    #endregion

    public virtual bool CanIgnoreSkillDirectDamage() => false;
    public virtual float GetDamageReducePct() => 0;
}