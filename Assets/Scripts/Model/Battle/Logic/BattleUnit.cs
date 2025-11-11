using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleUnit : IModel, IRecycle
{
    #region Inject注入
    [Inject] private IPoolManager PoolManager { get; set; }
    
    [Inject] private ILogManager LogManager { get; set; }
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] private BattleMomentConditionManager BattleMomentConditionManager { get; set; }
    [Inject] private ConfigManager ConfigManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    [Inject] private BattleUtil BattleUtil { get; set; }
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    [Inject] private BattleTypeManager BattleTypeManager { get; set; }
    
    [Inject] private BattleRecordManager BattleRecordManager { get; set; }
    
    [Inject] private IMessageManager MessageManager { get; set; }
    
    #endregion
    
    public BattleField Bf { get; set; }
    private HeroData HeroData { get; set; }
    public int EntityID { get; set; }
    public int SlotIndex { get; set; }
    protected BattleObjType ObjType { get; set; }
    private BattleProperty Property { get; set; }

    #region 携带的Buff,心法,宝器,当前释放的技能

    /// <summary>
    /// 携带的buff
    /// </summary>
    private DictAndList<int, BattleBuffBase> Buffs = new();
    
    /// <summary>
    /// 携带的心法
    /// </summary>
    private List<BattleHeartMethodBase> HeartMethods = new();
    
    /// <summary>
    /// 携带的宝器
    /// </summary>
    private List<BattleTreasureBase> Treasures = new();
    
    private Queue<BattleSkillBase> SkillSequence = new();

    #region 技能数据

    /// <summary>
    /// 携带的技能
    /// </summary>
    public TakeSkillDataManager TakeSkillDataManager { get; private set; }
    
    /// <summary>
    /// 技能预先数据
    /// </summary>
    public PreUseSkillDataManager PreUseSkillDataManager { get; private set; }
    
    /// <summary>
    /// 技能使用数据
    /// </summary>
    /// <returns></returns>
    public UseSkillDataManager UseSkillDataManager { get; private set; }
    
    #endregion
    
    public BattleSkillBase GetSkill()
    {
        if (SkillSequence.Any())
        {
            return SkillSequence.Peek();
        }
        
        return null;
    }
    
    public void AddUseSkill(int skillGuid, BattleUnit target, bool needCostResource = true, bool isRepeat = false)
    {
        PreUseSkillDataManager.TryAddSkillPreUseData(skillGuid);
        var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
        var skillBase = (BattleSkillBase)PoolManager.GetClass(BattleTypeManager.GetSkillType(skillID));
        skillBase.Init(skillGuid, this, target, needCostResource, isRepeat);
        SkillSequence.Enqueue(skillBase);
    }

    public void TryRemoveUseSkill(SkillRemoveMomentType type, DamageParamModel model = null)
    {
        if (SkillSequence.Any())
        {
            var skillBase = SkillSequence.Dequeue();
            if ((type == SkillRemoveMomentType.BeCounter) ||
                (skillBase.GetRemoveMomentList.Contains((int)type) && type == SkillRemoveMomentType.RoundEnd) ||
                (skillBase.GetRemoveMomentList.Contains((int)type) && type == SkillRemoveMomentType.AfterAction) ||
                (skillBase.GetRemoveMomentList.Contains((int)type) && type == SkillRemoveMomentType.BeforeNextAction && skillBase.CheckTriggerMoment(BattleMomentType.AfterAction)) ||
                (skillBase.GetRemoveMomentList.Contains((int)type) && type == SkillRemoveMomentType.NextRoundStart))
            {
                skillBase.SkillEnd();
                TriggerBuffSkillEnd();
                PreUseSkillDataManager.TryAddSkillPreUseDataBySkillEnd(skillBase.SkillGuid, type == SkillRemoveMomentType.BeCounter ? LastUseSkillState.BeCounter : LastUseSkillState.UseSuccess);
                UseSkillDataManager.AddUseSkillData(skillBase.SkillGuid, BattleLogicStateManager.Round, BattleLogicStateManager.ActionWheel, skillBase.ClashState);
                BattleLogicStateManager.AddRoundUsedSkillGuid(skillBase.SkillGuid);
                TryRepeatUseSkill(skillBase, model);
                PoolManager.RecycleClass(skillBase);
            }
        }
    }

    private void TriggerBuffSkillEnd()
    {
        foreach (var buff in GetBuffList())
        {
            buff.SkillEnd();
        }
    }
    
    #endregion
    
    public bool IsSelf { get; set; }
    public float ActionRadius { get; set; }
    public float ClashRadius { get; set; }
    public int Bgm { get; set; }
    public int Gr { get; private set; }
    
    private List<int> Variety = new();
    public bool CheckVariety(HeroVariety checkVariety) => Variety.Contains((int)checkVariety);
    public virtual void Init(BattleField bf, HeroData heroData)
    {
        Bf = bf;
        IsSelf = bf.Uid == 1;
        HeroData = heroData;
        SlotIndex = heroData.SlotIndex;
        BattleManager.ResetUnitToDict(this);
        Property = PoolManager.GetClass<BattleProperty>();
        Property.Init(heroData, this);
        AddRandomKey(Property.GetKeyProperty(BattleKeyType.KeyMax) + GetKeyProperty(BattleKeyType.KeyMaxEx), ChangeKeyReason.Init);
        TakeSkillDataManager = PoolManager.GetClass<TakeSkillDataManager>();
        TakeSkillDataManager.InitSkillData(heroData.WearSkillList);
        PreUseSkillDataManager = PoolManager.GetClass<PreUseSkillDataManager>();
        UseSkillDataManager = PoolManager.GetClass<UseSkillDataManager>();
        ActionTimes = 0;
        RoundBeDirectDamageTimes = 0;
        RoundAlreadyActionTimes = 0;
        IgnoreDirectKillingDamage = 0;
        TransformState = BattleUnitTransformState.None;
        foreach (var heartMethodID in HeroData.WearHeartMethodList)
        {
            var heartMethod = PoolManager.GetClass<BattleHeartMethodBase>();
            heartMethod.Init(heartMethodID, this);
            HeartMethods.Add(heartMethod);
        }
        foreach (var treasureID in HeroData.WearTreasureList)
        {
            var treasure = PoolManager.GetClass<BattleTreasureBase>();
            treasure.Init(treasureID, this);
            Treasures.Add(treasure);
        }

        ActionRadius = heroData.GetFightProperty_ActionRadius();
        ClashRadius = heroData.GetFightProperty_ClashRadius();
        Bgm = heroData.GetFightProperty_Bgm();
        Gr = heroData.GetJr();
        Variety.AddRange(heroData.GetFightProperty_Variety());
        InitTakeProp();
    }

    /// <summary>
    /// 回合开始
    /// </summary>
    public virtual void RoundStart()
    {
        //行动有关
        ActionTimes += 1;
        SpeedCounting = 0;
        ActionWheel = 0;
        ActionWheelOut = 0;
        
        //键有关
        IgnoreBeCounterByDamage = 0;
        IgnoreBeCounterByKeyTypeList.Clear();
        
        //伤害
        AccumulateDamageState = false;
        if (AccumulateDamageValue > 0)
        {
            if (ReduceHp(AccumulateDamageValue, DamageType.InDirect, 0, source: BattleSource.None))
            {
                
            }
        }
        AccumulateDamageValue = 0;

        if (BattleLogicStateManager.Round != 1 && NotRecoverQiNatural <=0)
        {
            RecoverGangQiNatural();
            RecoverXuanQiNatural();
            RecoverKeyNatural();
        }
    }

    /// <summary>
    /// 这一息结束
    /// </summary>
    public void OneActionWheelEnd()
    {
        ActionWheelOut = 0;
        ActionWheelIsAction = false;
    }
    
    /// <summary>
    /// 回合结束
    /// </summary>
    public virtual void RoundEnd()
    {
        //行动有关
        SpeedCounting = 0;
        ActionWheel = 0;
        ActionWheelOut = 0;
        RoundBeDirectDamageTimes = 0;
        RoundAlreadyActionTimes = 0;
        RoundBeDamageValue = 0;
        
        //键有关
        IgnoreBeCounterByDamage = 0;
        IgnoreBeCounterByKeyTypeList.Clear();
        
        RoundBeDirectDamagedOpponentList.Clear();
        RoundBeDirectKillAttackOpponentList.Clear();
    }

    #region 战斗扳机

    private List<IBattleMoment> TempBattleMoment = new();
     
    /// <summary>
    /// 避免存在两个技能  一直触发老的技能
    /// </summary>
    /// <param name="isLastSkill"></param>
    /// <returns></returns>
    public List<IBattleMoment> GetBattleMoment(bool isLastSkill = true)
    {
        TempBattleMoment.Clear();
        TempBattleMoment.AddRange(Treasures);
        TempBattleMoment.AddRange(HeartMethods);
        TempBattleMoment.AddRange(GetBuffList());
        if (isLastSkill)
        {
            var skillBase = GetSkill();
            if (skillBase != null)
            {
                TempBattleMoment.Add(skillBase);
            }
        }
        else
        {
            if (SkillSequence.Any())
            {
                TempBattleMoment.Add(SkillSequence.Last());
            }
        }
        
        return TempBattleMoment;
    }

    #endregion

    #region 战斗属性改变机制

    private List<IBattlePropertyChanged> TempBattlePropertyChanged = new();
    public List<IBattlePropertyChanged> GetBattlePropertyChanged()
    {
        TempBattlePropertyChanged.Clear();
        TempBattlePropertyChanged.AddRange(Treasures);
        TempBattlePropertyChanged.AddRange(HeartMethods);
        TempBattlePropertyChanged.AddRange(GetBuffList());
        return TempBattlePropertyChanged;
    }

    #endregion
    

    protected virtual void Die()
    {
        
    }
    
    #region 属性
    
    public float ChangeProperty(BattlePropertyType propType, float originPropValue, BattleSource source = BattleSource.None)
    {
        if (propType == BattlePropertyType.GangQiPct)
        {
            return Property.ChangeProperty(BattlePropertyType.GangQi,
                GetProperty(BattlePropertyType.MaxGangQi) * originPropValue, source);
        }
        
        if (propType == BattlePropertyType.XuanQiPct)
        {
            return Property.ChangeProperty(BattlePropertyType.XuanQi,
                GetProperty(BattlePropertyType.MaxXuanQi) * originPropValue, source);
        }

        var finalPropValue = originPropValue;
        if (propType == BattlePropertyType.GangQi && finalPropValue > 0 && source == BattleSource.Skill)
        {
            finalPropValue = Math.Max(finalPropValue + GetProperty(BattlePropertyType.RecoverGangQiBySkillOffset), 0);
        }
        
        if (propType == BattlePropertyType.XuanQi && finalPropValue > 0 && source == BattleSource.Skill)
        {
            finalPropValue = Math.Max(finalPropValue + GetProperty(BattlePropertyType.RecoverXuanQiBySkillOffset), 0);
        }
        finalPropValue = Property.ChangeProperty(propType, finalPropValue, source);
        foreach (var buff in GetBuffList())
        {
            buff.ChangeProperty(propType, originPropValue, finalPropValue, source);
        }
        return finalPropValue;
    }
    
    /// <summary>
    /// 直接改变多少 不走增益或者减益
    /// </summary>
    /// <param name="propType"></param>
    /// <param name="propValue"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public bool ChangeProperty_Abs(BattlePropertyType propType, float propValue, BattleSource source = BattleSource.None)
    {
        return Property.ChangeProperty_Abs(propType, propValue, source);
    }
    
    public bool SetProperty(BattlePropertyType propType, float propValue, BattleSource source = BattleSource.None)
    {
        return Property.SetProperty(propType, propValue, source);
    }

    public float GetProperty(BattlePropertyType propType)
    {
        //彻buff单独写
        if (propType == BattlePropertyType.Power || propType == BattlePropertyType.Tech)
        {
            var buff = GetBuff(GameConst.Battle.Buff30301);
            if (buff != null)
            {
                return buff.GetProperty(propType);
            }
        }
        
        return Property.GetProperty(propType);
    }

    public float GetPropertyPct(BattlePropertyType propType)
    {
        return Property.GetPropertyPct(propType);
    }
    
    public void ForceRefreshPropertyLimit() => Property.TryAdjustLimit();
    public bool IsAlive() => GetProperty(BattlePropertyType.Hp) > 0;
    
    /// <summary>
    /// 剩余行动次数
    /// </summary>
    public int ActionTimes { get; private set; }
    /// <summary>
    /// 本回合被直接伤害几次
    /// </summary>
    public int RoundBeDirectDamageTimes { get; private set; }
    /// <summary>
    /// 本回合行动过几次
    /// </summary>
    public int RoundAlreadyActionTimes { get; private set; }
    /// <summary>
    /// 这一息是否行动过了
    /// </summary>
    public bool ActionWheelIsAction { get; private set; }
    public void EndAction()
    {
        foreach (var buff in GetBuffList())
        {
            buff.EndAction();
        }
        RoundAlreadyActionTimes++;
        ActionTimes--;
        BeCounter = false;
        ActionWheelIsAction = true;
    }
    
    public bool TryCalculateNextActionWheel()
    {
        if (ActionTimes <= 0)
        {
            return false;
        }

        if (BattleLogicBehaviourManager.BattleBehaviourRes.ContainsKey(EntityID))
        {
            return false;
        }

        var skill = GetSkill();
        if (skill != null && skill.GetRemoveMomentList.Count == 1 &&
            skill.GetRemoveMomentList[0] == (int)SkillRemoveMomentType.NextRoundStart)
        {
            return false;
        }
        
        ActionWheel = BattleLogicStateManager.ActionWheel + 1;
        return true;
    }

    public int NotRecoverQiNatural;
    public void AddNotRecoverQiNatural(int state) => NotRecoverQiNatural += state;
    
    public float SpeedCounting;
    //下一行动息值
    public int ActionWheel;
    //息溢值
    public int ActionWheelOut;
    //是否被破招了
    private bool BeCounter;
    public bool GetBeCounter() => BeCounter;
    //public void SetBeCounter(bool state) => BeCounter = state;

    /// <summary>
    /// 不会被破招
    /// </summary>
    private int DontBeCounter;

    public void SetDontBeCounter(int value)
    {
        DontBeCounter += value;
    }
    /// <summary>
    /// 不会被武杀式破招
    /// </summary>
    private int DontBeCounterByPowerKilling;
    public void SetDontBeCounterByPowerKilling(int state) => DontBeCounterByPowerKilling += state;
    /// <summary>
    /// 不会被术杀式破招
    /// </summary>
    private int DontBeCounterByArtKilling;
    public void SetDontBeCounterByArtKilling(int state) => DontBeCounterByArtKilling += state;
    /// <summary>
    /// 不会被破招的键的列表
    /// </summary>
    private List<BattleKeyType> IgnoreBeCounterByKeyTypeList = new();
    public void AddIgnoreBeCounterKey(BattleKeyType key) => IgnoreBeCounterByKeyTypeList.Add(key);
    /// <summary>
    /// 未受到多少此伤害前不会被破招
    /// </summary>
    private int IgnoreBeCounterByDamage;
    public void AddIgnoreBeCountByDamage(int count) => IgnoreBeCounterByDamage += count;
    //public void AddIgnoreBeCountByCount(int count) => IgnoreBeCounterByDamage += count;
    /// <summary>
    /// 不会被未带有↑类留劲buff的破招
    /// </summary>
    private int IgnoreTargetNotHasUpBuff;
    public void AddIgnoreTargetNotHasUpBuff(int state) => IgnoreTargetNotHasUpBuff += state;
    /// <summary>
    /// 不会被未带有↓类留劲buff的破招
    /// </summary>
    private int IgnoreTargetNotHasDownBuff;
    public void AddIgnoreTargetNotHasDownBuff(int state) => IgnoreTargetNotHasDownBuff += state;
    /// <summary>
    /// 不会被未带有←类留劲buff的破招
    /// </summary>
    private int IgnoreTargetNotHasLeftBuff;
    public void AddIgnoreTargetNotHasLeftBuff(int state) => IgnoreTargetNotHasLeftBuff += state;
    /// <summary>
    /// 不会被未带有→类留劲buff的破招
    /// </summary>
    private int IgnoreTargetNotHasRightBuff;
    public void AddIgnoreTargetNotHasRightBuff(int state) => IgnoreTargetNotHasRightBuff += state;
    /// <summary>
    /// 不会被招式未带有↑键的敌手破招
    /// </summary>
    private int IgnoreTargetSkillNotHasUpKey;
    public void AddIgnoreTargetSkillNotHasUpKey(int state) => IgnoreTargetSkillNotHasUpKey += state;
    /// <summary>
    /// 不会被招式未带有↓键的敌手破招
    /// </summary>
    private int IgnoreTargetSkillNotHasDownKey;
    public void AddIgnoreTargetSkillNotHasDownKey(int state) => IgnoreTargetSkillNotHasDownKey += state;
    /// <summary>
    /// 不会被招式未带有←键的敌手破招
    /// </summary>
    private int IgnoreTargetSkillNotHasLeftKey;
    public void AddIgnoreTargetSkillNotHasLeftKey(int state) => IgnoreTargetSkillNotHasLeftKey += state;
    /// <summary>
    /// 不会被招式未带有→键的敌手破招
    /// </summary>
    private int IgnoreTargetSkillNotHasRightKey;
    public void AddIgnoreTargetSkillNotHasRightKey(int state) => IgnoreTargetSkillNotHasRightKey += state;
    /// <summary>
    /// 尝试被破招
    /// </summary>
    public bool TryBeCounter(int attackerID)
    {
        var attack = BattleManager.GetUnit(attackerID);
        var attackSkill = attack.GetSkill();
        var skillID = attackSkill.SkillID;
        var costKey = attackSkill.GetKeyCostList;
        //破招失败
        if (DontBeCounter > 0)
        { 
            return false;
        }

        if (DontBeCounterByPowerKilling > 0 && BattleUtil.GetSkillTypeBySkillID(skillID) == SkillType.PowerKilling)
        {
            return false;
        }

        if (DontBeCounterByArtKilling > 0 && BattleUtil.GetSkillTypeBySkillID(skillID) == SkillType.ArtKilling)
        {
            return false;
        }
        
        if (IgnoreBeCounterByKeyTypeList.Any(hasKey => costKey.Contains((int)hasKey)))
        {
            return false;
        }
        //不会被未带有↑的留劲Buff破招
        if (IgnoreTargetNotHasUpBuff > 0 && !BattleBuffManager.CheckTargetHasUpFirstSkillBuff(attackerID))
        {
            return false;
        }
        //不会被未带有↓的留劲Buff破招
        if (IgnoreTargetNotHasDownBuff > 0 && !BattleBuffManager.CheckTargetHasDownFirstSkillBuff(attackerID))
        {
            return false;
        }
        //不会被未带有←的留劲Buff破招
        if (IgnoreTargetNotHasLeftBuff > 0 && !BattleBuffManager.CheckTargetHasLeftFirstSkillBuff(attackerID))
        {
            return false;
        }
        //不会被未带有→的留劲Buff破招
        if (IgnoreTargetNotHasRightBuff > 0 && !BattleBuffManager.CheckTargetHasRightFirstSkillBuff(attackerID))
        {
            return false;
        }
        
        //不会被招式未带有↑键的敌手破招
        if (IgnoreTargetSkillNotHasUpKey > 0 && !costKey.Contains((int)BattleKeyType.KeyUp))
        {
            return false;
        }
        //不会被招式未带有↓键的敌手破招
        if (IgnoreTargetSkillNotHasDownKey > 0 && !costKey.Contains((int)BattleKeyType.KeyDown))
        {
            return false;
        }
        //不会被招式未带有←键的敌手破招
        if (IgnoreTargetSkillNotHasLeftKey > 0 && !costKey.Contains((int)BattleKeyType.KeyLeft))
        {
            return false;
        }
        //不会被招式未带有→键的敌手破招
        if (IgnoreTargetSkillNotHasRightKey > 0 && !costKey.Contains((int)BattleKeyType.KeyRight))
        {
            return false;
        }
        
        //破招抵免buff
        var buff = GetBuff(GameConst.Battle.ImmunityCounterBuffID);
        if (buff is { LayerCount: > 0 })
        {
            buff.ReduceLayerCount(1);
            return false;
        }
 
        if (IgnoreBeCounterByDamage > 0)
        {
            IgnoreBeCounterByDamage--;
            return false;
        }

        BeCounter = true;
        return true;
    }
    
    //改变息
    public void ChangeActionWheel(int value)
    {
        if (value > 0)
        {
            var fastMax = BattleLogicStateManager.GetAfterStartActionWheel
                ? BattleLogicStateManager.ActionWheel + 1
                : BattleLogicStateManager.ActionWheel;
        
            if (fastMax <= 0)
            {
                fastMax = 1;
            }
            
            if (ActionWheel - value <= fastMax)
            {
                ActionWheelOut += fastMax - ActionWheel + value;
                ActionWheel = fastMax;
            }
            else
            {
                ActionWheel -= value;
            }
        }
        else if (value < 0)
        {
            if (ActionWheelOut + value >= 0)
            {
                ActionWheelOut += value;
            }
            else
            {
                ActionWheelOut = 0;
                ActionWheel -= (ActionWheelOut + value);
            }
        }
    }

    /// <summary>
    /// 延迟受伤
    /// </summary>
    private bool AccumulateDamageState;
    public void SetAccumulateDamage() => AccumulateDamageState = true;
    
    private float AccumulateDamageValue;
    /// <summary>
    /// 本回合对自己造成过直接伤害的对手ID
    /// </summary>
    private List<int> RoundBeDirectDamagedOpponentList = new();
    public bool CheckRoundBeSameDirectDamaged(int attackID) => RoundBeDirectDamagedOpponentList.Contains(attackID);
    /// <summary>
    /// 本回合对自己使用过直接杀式攻击的对手ID
    /// </summary>
    private List<int> RoundBeDirectKillAttackOpponentList = new();

    private int IgnoreDirectKillingDamage;
    public void AddIgnoreDirectKillingDamage(int state) => IgnoreDirectKillingDamage += state;
    public bool CheckRoundBeDirectKillAttack(int attackID)
    {
        if (attackID == 0)
        {
            return RoundBeDirectKillAttackOpponentList.Count > 0;
        }
        
        return RoundBeDirectKillAttackOpponentList.Contains(attackID);
    }

    /// <summary>
    /// 本回合受到过的伤害
    /// </summary>
    public float RoundBeDamageValue { get; private set; }
    
    public virtual void BeDamage(ref DamageParamModel model)
    {
        if (model.HitDamageType == DamageType.Direct)
        {
            if (IgnoreDirectKillingDamage > 0 && BattleUtil.SkillIsKillingStyle(model.AttackSkillID))
            {
                model.HitTruthDamageValue = 0;
                model.HitHpValue = 0;
                model.HitShieldValue = 0;
                model.HitArmorValue = 0;
            }
            
            RoundBeDirectKillAttackOpponentList.Add(model.AttackID);

            var attacker = BattleManager.GetUnit(model.AttackID);
            var attackerSkill = attacker.GetSkill();
            var damageValue = 0.0f;
            if (attackerSkill != null)
            {
                if (attackerSkill.IsTrueDamage(model))
                {
                    damageValue = model.HitTruthDamageValue;
                }
                else
                {
                    damageValue = model.HitHpValue;
                    if (model.HitShieldValue > 0)
                    {
                        ReduceBuffLayerCount(GameConst.Battle.ShieldBuffID, model.HitShieldValue.ToInt());
                    }
                
                    if (model.HitArmorValue > 0)
                    {
                        ReduceBuffLayerCount(GameConst.Battle.ArmorBuffID, model.HitArmorValue.ToInt());
                    }
                }
            }
            
            //如果在累计伤害, 不算掉血
            if (AccumulateDamageState)
            {
                AccumulateDamageValue += damageValue;
            }
            else
            {
                RoundBeDamageValue += damageValue;
                if (damageValue > 0)
                {
                    RoundBeDirectDamagedOpponentList.Add(model.AttackID);
                    if (ReduceHp(damageValue, DamageType.Direct, model.AttackID, source: BattleSource.Skill))
                    {
                        
                    }
                }
            }
        }
        else if (model.HitDamageType == DamageType.InDirect)
        {
            var attacker = BattleManager.GetUnit(model.AttackID);
            var attackerSkill = attacker.GetSkill();
            var damageValue = 0.0f;
            if (attackerSkill != null)
            {
                damageValue = attackerSkill.IsTrueDamage(model) ? model.HitTruthDamageValue : model.HitHpValue;
            }
            RoundBeDamageValue += damageValue;
            if (damageValue > 0)
            {
                if (ReduceHp(damageValue, DamageType.InDirect, model.AttackID, source: BattleSource.Skill))
                {
                    
                }
            }
        }
    }
    
    /// <summary>
    /// 击杀的列表ID
    /// </summary>
    public List<int> KillUnitList = new();
    public void AddKillID(int entityID) => KillUnitList.Add(entityID);

    public virtual void SetHp(float setValue, BattleSource source = BattleSource.None)
    {
        SetProperty(BattlePropertyType.Hp, setValue, source);
    }
    
    /// <summary>
    /// 加血
    /// </summary>
    /// <param name="healValue"></param>
    /// <param name="???"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public virtual float HealHp(float healValue, BattleSource source = BattleSource.None)
    {
        return ChangeProperty(BattlePropertyType.Hp, healValue, source);
    }
    
    /// <summary>
    /// 扣血
    /// </summary>
    /// <param name="reduceHp"></param>
    /// <param name="damageType"></param>
    /// <param name="attackID"></param>
    /// <param name="triggerBeHitEventModel"></param>
    /// <param name="source"></param>
    /// <returns></returns>
    public virtual bool ReduceHp(float reduceHp, DamageType damageType, int attackID, bool triggerBeHitEventModel = true, BattleSource source = BattleSource.None)
    {
        //增加本回合受到直接伤害的次数
        if (damageType == DamageType.Direct && reduceHp > 0)
        {
            RoundBeDirectDamageTimes++;
        }
        ChangeProperty(BattlePropertyType.Hp, -reduceHp, source);
        OnHpChanged();
        var isDie = GetProperty(BattlePropertyType.Hp) <= 0;
        if (isDie)
        {
            var attack = BattleManager.GetUnit(attackID);
            attack.AddKillID(EntityID);
            Die();
        }
        TriggerBuffBeAttack(reduceHp, damageType, attackID);
        if (triggerBeHitEventModel)
        {
            TriggerBeHitEventModel(reduceHp, damageType, attackID);
        }
        //是否要清理所有buff
        
        return isDie;
    }

    private void OnHpChanged()
    {
        foreach (var buff in GetBuffList())
        {
            buff.HpChanged();
        }
    }

    /// <summary>
    /// 一般会触发直接关系的 像铁索连环加在对方身上 自己受伤时获取不到就用事件处理
    /// </summary>
    /// <param name="reduceHp"></param>
    /// <param name="damageType"></param>
    /// <param name="attackID"></param>
    private void TriggerBeHitEventModel(float reduceHp, DamageType damageType, int attackID)
    {
        var model = PoolManager.GetClass<UnitBeHitEventModel>();
        model.HitID = EntityID;
        model.DamageValue = reduceHp;
        model.DamageType = damageType;
        model.AttackID = attackID;
        MessageManager.DispatchMsg(model);
        PoolManager.RecycleClass(model);
    }
    
    #region 技能方法

    public float GetSkillDamageRate(SkillDataGetType getType, int skillGuid = 0, MomentParamModel paramModel = null)
    {
        switch (getType)
        {
            case SkillDataGetType.DamagePreview:
                if (skillGuid > 0)
                {
                    var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
                    var damageBase = PreUseSkillDataManager.GetSkillPreUseDamage(skillGuid);
                    ChangeModelTrySetBaseWellyRate(skillGuid, ref damageBase);
                    var skillType = BattleUtil.GetSkillTypeBySkillID(skillID);
                    var tempSkillAddWelly = GetProperty(BattlePropertyType.TempSkillAddWellyRate);
                    var buffAddWelly = GetChangeModelGetAddWellyRate(skillGuid);
                    var skillWellyEffectDelta = PreUseSkillDataManager.GetSkillWellyEffect(skillGuid);
                    var buffWellyEffectDelta = GetChangeModelGetAddWellyEffect(skillGuid);
                    switch (skillType)
                    {
                        case SkillType.None:
                            break;
                        case SkillType.PowerKilling:
                            tempSkillAddWelly += GetProperty(BattlePropertyType.TempPowerSkillAddWellyRate);
                            break;
                        case SkillType.ArtKilling:
                            tempSkillAddWelly += GetProperty(BattlePropertyType.TempArtSkillAddWellyRate);
                            break;
                        case SkillType.TechniqueImperialStyle:
                            break;
                        case SkillType.SpellFormula:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                
                    return damageBase + (tempSkillAddWelly + buffAddWelly) * (skillWellyEffectDelta + buffWellyEffectDelta);
                }
                break;
            case SkillDataGetType.DamageBase:
                if (skillGuid > 0)
                {
                    var damageBase = PreUseSkillDataManager.GetSkillPreUseDamage(skillGuid);
                    ChangeModelTrySetBaseWellyRate(skillGuid, ref damageBase);
                    return damageBase;
                }

                var skillBase = GetSkill();
                if (skillBase != null)
                {
                    var damageBase = PreUseSkillDataManager.GetSkillPreUseDamage(skillBase.SkillGuid);
                    ChangeModelTrySetBaseWellyRate(skillGuid, ref damageBase);
                    return damageBase;
                }
                break;
            case SkillDataGetType.DamageCurr:
                var skill = GetSkill();
                if (skill != null)
                {
                    var damageBase = skill.GetSkillDamageRate;
                    ChangeModelTrySetBaseWellyRate(skill.SkillGuid, ref damageBase);
                    var skillType = skill.GetSKillType;
                    var tempSkillAddWelly = GetProperty(BattlePropertyType.TempSkillAddWellyRate);
                    var buffAddWelly = GetChangeModelGetAddWellyRate(skill.SkillGuid);
                    var skillWellyEffectDelta = skill.GetSkillWellyEffect;
                    var buffWellyEffectDelta = GetChangeModelGetAddWellyEffect(skill.SkillGuid);
                    switch (skillType)
                    {
                        case SkillType.None:
                            break;
                        case SkillType.PowerKilling:
                            tempSkillAddWelly += GetProperty(BattlePropertyType.TempPowerSkillAddWellyRate);
                            break;
                        case SkillType.ArtKilling:
                            tempSkillAddWelly += GetProperty(BattlePropertyType.TempArtSkillAddWellyRate);
                            break;
                        case SkillType.TechniqueImperialStyle:
                            break;
                        case SkillType.SpellFormula:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                
                    var allAddWelly = tempSkillAddWelly + buffAddWelly + skill.GetSkillAddWellyRate(paramModel);
                    ChangeModelTrySetAddWellyRate(skill.SkillGuid, ref allAddWelly);
                    return damageBase + allAddWelly * (skillWellyEffectDelta + buffWellyEffectDelta);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(getType), getType, null);
        }
        
        return 0;
    }

    #region 状态改变技能属性

    //获取威力改变
    private float GetChangeModelGetAddWellyRate(int skillGuid)
    {
        return GetBattlePropertyChanged().Sum(changeModel => changeModel.GetAddWellyRate(skillGuid));
    }
    //获取威力效果
    private float GetChangeModelGetAddWellyEffect(int skillGuid)
    {
        return GetBattlePropertyChanged().Sum(changeModel => changeModel.GetAddWellyEffect(skillGuid));
    }
    
    //尝试设置威力基数
    private void ChangeModelTrySetBaseWellyRate(int skillGuid, ref float value)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.TrySetBaseWellyRate(skillGuid, ref value);
        }
    }
    
    //尝试设置威力增长
    private void ChangeModelTrySetAddWellyRate(int skillGuid, ref float value)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.TrySetAddWellyRate(skillGuid, ref value);
        }
    }

    #endregion
    
    public int GetSkillID()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return 0;

        return skillBase.SkillID;
    }
    
    public string GetSkillAniName()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return string.Empty;

        return skillBase.GetSkillAniName();
    }
    
    public SkillType GetSkillType()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return SkillType.None;

        return skillBase.GetSKillType;
    }

    public DamageType GetSkillDamageType()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return DamageType.None;

        return skillBase.GetDamageType;
    }

    public void ReturnSkillResourceCost(bool returnGangQi = false, bool returnXuanQi = false, bool returnKey = false)
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return;

        skillBase.ReturnSkillResourceCost(returnGangQi, returnXuanQi, returnKey);
    }
    
    #endregion

    public void ForceChangeTarget(int newTargetID)
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return;
        var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(EntityID);
        behaviour.TargetID = newTargetID;
        var newTarget = BattleManager.GetUnit(newTargetID);
        skillBase.SetTarget(newTarget);
    }

    private void RecoverGangQiNatural()
    {
        ChangeProperty(BattlePropertyType.GangQi, GetProperty(BattlePropertyType.GangQiRecNatural), BattleSource.Natural);
    }
    
    private void RecoverXuanQiNatural()
    {
        ChangeProperty(BattlePropertyType.XuanQi, GetProperty(BattlePropertyType.XuanQiRecNatural), BattleSource.Natural);
    }
    
    public void SetActionWheelToNow()
    {
        ActionWheel = BattleLogicStateManager.ActionWheel;
    }

    public void AddActionTimes(int times)
    {
        ActionTimes += times;
    }

    #region 键相关

    public List<int> GetAllKeyTypeList() => Property.GetAllKeyTypeList();
    public int GetKeyCount(BattleKeyType keyType, bool isLocked = false) => Property.GetKeyCount(keyType, isLocked);
    
    public void AddRandomKey(int count, ChangeKeyReason reason = ChangeKeyReason.None)
    {
        var getKey = Util.GetRandomKey(count);
        foreach (var key in getKey)
        {
            ChangeKey(key, 1, reason);
        }
    }
    public int GetKeyProperty(BattleKeyType keyType) => Property.GetKeyProperty(keyType);
    public int ChangeKeyProperty(BattleKeyType keyType, int count, ChangeKeyReason reason = ChangeKeyReason.None) => Property.ChangeKeyProperty(keyType, count, reason);
    public void RemoveRandomKey(int count, ChangeKeyReason reason = ChangeKeyReason.None)
    {
        var allKey = GetAllKeyTypeList().Clone();
        var removeList = Util.GetRandomNoSame(allKey, Util.GetSameChanceList(allKey.Count), count);
        foreach (var removeKeyType in removeList)
        {
            ChangeKey((BattleKeyType)removeKeyType, -1, reason);
        }
    }

    public List<BattleKey> CheckKeyLimit() => Property.CheckKeyLimit();

    public List<BattleKey> ChangeKey(BattleKeyType keyType, int value, ChangeKeyReason reason = ChangeKeyReason.None)
    {
        var deltaCount = Property.ChangeKey(keyType, value, reason);
        foreach (var buff in GetBuffList())
        {
            if (value > 0)
            {
                buff.KeyAdd(keyType, deltaCount, reason);
            }
            else
            {
                buff.KeyReduce(keyType, deltaCount, reason);
            }
        }

        return deltaCount;
    }
    public int GetAllKeyCount() => Property.GetAllKeyCount();
    public int GetKeyPropertyMax() => Property.GetKeyPropertyMax();
    public void RemoveAllKey() => Property.RemoveAllKey();
    public List<BattleKey> LockRandomKey(int count) => Property.LockRandomKey(count);
    public BattleKey UnlockKey(int guid) => Property.UnlockKey(guid);
    public List<BattleKey> PollutionRandomKey(int count) => Property.PollutionRandomKey(count);
    public BattleKey UnPollutionKey(int guid) => Property.UnPollutionKey(guid);
    private void RecoverKeyNatural()
    {
        AddRandomKey(GetKeyProperty(BattleKeyType.KeyRecoverNatural), ChangeKeyReason.NaturalRecover);
    }
    
    #endregion

    public (float, float) GetSkillQiCost(SkillDataGetType getType, int skillGuid = 0)
    {
        switch (getType)
        {
            case SkillDataGetType.CostPreview:
                if (skillGuid > 0)
                {
                    var gangQiCost = PreUseSkillDataManager.GetSkillPreUseGangQiCost(skillGuid);
                    var xuanQiCost = PreUseSkillDataManager.GetSkillPreUseXuanQiCost(skillGuid);
                    foreach (var buff in GetBuffList())
                    {
                        (gangQiCost, xuanQiCost) = buff.ChangeResourceCost(gangQiCost, xuanQiCost);
                    }
                    return (GetGangQiReduce(gangQiCost), GetXuanQiReduce(xuanQiCost));
                }
                break;
            case SkillDataGetType.CheckCost:
                var skill = GetSkill();
                if (skill != null)
                {
                    var gangQiCost = skill.GetGangQiCost();
                    var xuanQiCost = skill.GetXuanQiCost();
                    return (GetGangQiReduce(gangQiCost), GetXuanQiReduce(xuanQiCost));
                }
                break;
            case SkillDataGetType.ReleaseCost:
                skill = GetSkill();
                if (skill != null)
                {
                    var gangQiCost = skill.GetGangQiCost();
                    var xuanQiCost = skill.GetXuanQiCost();
                    return (gangQiCost, xuanQiCost);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(getType), getType, null);
        }

        return (0, 0);
    }

    public List<int> GetSkillKeyCost(SkillDataGetType getType, int skillGuid = 0)
    {
        switch (getType)
        {
            case SkillDataGetType.KeyPreview:
                if (skillGuid > 0)
                {
                    return PreUseSkillDataManager.GetSkillPreUseKeyCost(skillGuid);
                }
                break;
            case SkillDataGetType.CheckKey:
            case SkillDataGetType.ReleaseKey:
                var skill = GetSkill();
                if (skill != null)
                {
                    return skill.GetKeyCostList;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(getType), getType, null);
        }

        return new List<int>();
    }
    
    /// <summary>
    /// 预处理的属性回复或者减少方法
    /// </summary>
    /// <param name="propValue"></param>
    /// <returns></returns>
    public float GetGangQiRecover(float propValue) => Property.GetGangQiRecover(propValue);
    public float GetGangQiReduce(float propValue) => Property.GetGangQiReduce(propValue);
    public float GetXuanQiRecover(float propValue) => Property.GetXuanQiRecover(propValue);
    public float GetXuanQiReduce(float propValue) => Property.GetXuanQiReduce(propValue);
    
    
    #endregion

    #region 技能方法
    
    /// <summary>
    /// 判断技能能否决定_消耗
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <returns></returns>
    public bool CheckSkillCanDoDesition_Cost(int skillGuid)
    {
        var (costGangQi, costXuanQi) = GetSkillQiCost(SkillDataGetType.CostPreview, skillGuid);
        var hasGangQi = GetProperty(BattlePropertyType.GangQi);
        if (hasGangQi < costGangQi)
            return false;
        
        var hasXuanQi = GetProperty(BattlePropertyType.XuanQi);
        if (hasXuanQi < costXuanQi)
            return false;
        
        foreach (var (keyType, keyCount) in Util.KeyListToDictionary(GetSkillKeyCost(SkillDataGetType.KeyPreview, skillGuid)))
        {
            var hasKey = GetKeyCount((BattleKeyType)keyType, true);
            if (hasKey < keyCount)
                return false;
        }
        
        return true;
    }
    
    
    /// <summary>
    /// 判断技能能否决定_逻辑
    /// </summary>
    /// <returns></returns>
    public bool CheckSkillCanDoDesition_Logic(int skillGuid, BattleUnit target)
    {
        if (!target.IsAlive())
        {
            return false;
        }

        if (target.HasBuffMechanism(BuffMechanism.NotBeActionTarget) && !target.HasBuffMechanism(BuffMechanism.NotEffectGainBuff))
        {
            return false;
        }
        
        //对方有嘲讽 且不是他自己
        var opponentList = BattleManager.GetAllOpponentUnit(EntityID, true);
        if (opponentList.Any(unit => unit.HasBuffMechanism(BuffMechanism.Mockery) && !target.HasBuffMechanism(BuffMechanism.NotEffectGainBuff)) && !target.HasBuffMechanism(BuffMechanism.Mockery))
        {
            return false;
        }

        var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
        var skillConfig = ConfigManager.GetBattleSkillConfig(skillID);
        if (skillConfig.CheckSkillDoDesition.Count > 0)
        {
            if (skillConfig.CheckSkillDoDesitionRelation == 1 && skillConfig.CheckSkillDoDesition.All(conditionID =>
                    BattleMomentConditionManager.GetCondition(conditionID, this, target, skillGuid, null)))
            {
                return true;
            }

            if (skillConfig.CheckSkillDoDesitionRelation == 2 && skillConfig.CheckSkillDoDesition.Any(conditionID =>
                    BattleMomentConditionManager.GetCondition(conditionID, this, target, skillGuid, null)))
            {
                return true;
            }
        }
        
        return GetBuffList().All(buff => buff.CheckSkillCanUse(skillGuid, target));
    }
    
    /// <summary>
    /// 检查技能是否能释放成功
    /// </summary>
    /// <returns></returns>
    public bool CheckReleaseSkillEnough()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return false;

        var (costGangQi, costXuanQi) = GetSkillQiCost(SkillDataGetType.CheckCost);
        var hasGangQi = GetProperty(BattlePropertyType.GangQi);
        if (hasGangQi < costGangQi)
            return false;
        
        var hasXuanQi = GetProperty(BattlePropertyType.XuanQi);
        if (hasXuanQi < costXuanQi)
            return false;
        
        foreach (var (keyType, keyCount) in Util.KeyListToDictionary(GetSkillKeyCost(SkillDataGetType.CheckKey)))
        {
            if (keyType == (int)BattleKeyType.KeyUp && HasBuffMechanism(BuffMechanism.LockUpKey))
            {
                return false;
            }
            
            if (keyType == (int)BattleKeyType.KeyDown && HasBuffMechanism(BuffMechanism.LockDownKey))
            {
                return false;
            }
            
            if (keyType == (int)BattleKeyType.KeyLeft && HasBuffMechanism(BuffMechanism.LockLeftKey))
            {
                return false;
            }
            
            if (keyType == (int)BattleKeyType.KeyRight && HasBuffMechanism(BuffMechanism.LockRightKey))
            {
                return false;
            }
            
            var hasKey = GetKeyCount((BattleKeyType)keyType, false);
            if (hasKey < keyCount)
                return false;
        }
        
        return true;
    }
    
    /// <summary>
    /// 消耗技能的资源
    /// </summary>
    public (float, float, List<int>) CostSkillNeedResource()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
        {
            return (0, 0, new List<int>());
        }

        if (!skillBase.NeedCostResource)
        {
            return (0, 0, new List<int>());
        }

        var (gangQiCost, xuanQiCost) = GetSkillQiCost(SkillDataGetType.ReleaseCost);
        ChangeProperty(BattlePropertyType.GangQi, -gangQiCost, BattleSource.Skill);
        ChangeProperty(BattlePropertyType.XuanQi, -xuanQiCost, BattleSource.Skill);
        var keyCost = GetSkillKeyCost(SkillDataGetType.ReleaseKey);
        foreach (var (keyType, keyCount) in Util.KeyListToDictionary(keyCost))
        {
            ChangeKey((BattleKeyType)keyType, -keyCount, ChangeKeyReason.SkillCost);
        }
        
        skillBase.SetTruthSkillCost(gangQiCost, xuanQiCost, keyCost);
        return (gangQiCost, xuanQiCost, keyCost);
    }

    /// <summary>
    /// BuffMechanism, float
    /// </summary>
    private Dictionary<int, float> BuffChangeDamageDic = new();
    
    /// <summary>
    /// 获取伤害值
    /// </summary>
    /// <param name="target"></param>
    /// <param name="damageType"></param>
    /// <param name="damageSource"></param>
    /// <param name="damageRate"></param>
    /// <param name="paramModel"></param>
    /// <returns>折前伤害，打的血量，盾，甲</returns>
    public (float, float, float, float) GetSkillDamageValue(BattleUnit target, DamageType damageType, BattleSource damageSource, float damageRate, DamageParamModel paramModel = null)
    {
        var skillType = GetSkillType();

        var skillBase = GetSkill();
        var skillDamageIncrease = 0.0f;
        if (skillBase != null)
        {
            //技能伤害百分比  buff伤害百分比增伤
            skillDamageIncrease = skillBase.GetSkillAddDamageRate(paramModel) + GetBuffList().Sum(buff => buff.AddSkillDamageRate(skillBase.SkillGuid));
        }
        
        var armorPiercing = 0.0f;
        if (skillBase != null)
        {
            armorPiercing = skillBase.GetSkillArmorPiercing;
        }
        
        if (skillType == SkillType.PowerKilling)
        {
            var power = GetProperty(BattlePropertyType.Power);
            var skillDamageRateSum = damageRate;
            var skillDamageRateFloor = GetProperty(BattlePropertyType.SkillDamageRateFloor);
            var damageReducePct = target.GetProperty(BattlePropertyType.DamageReducePct);
            var killDamageReduceInt = target.GetProperty(BattlePropertyType.KillingDamageReduceInt);
            var defendValue = target.GetProperty(BattlePropertyType.Defend);
            var truthDamage = Math.Max(0, power * skillDamageRateSum * (1 + skillDamageRateFloor + skillDamageIncrease));
            var reduceShieldValue = 0.0f;
            var shieldBuff = GetBuff(GameConst.Battle.ShieldBuffID);
            if (shieldBuff != null)
            {
                var shield = shieldBuff.LayerCount;
                if (truthDamage >= shield)
                {
                    reduceShieldValue = shield;
                }
                else
                {
                    reduceShieldValue = truthDamage;
                }
            }

            var reduceArmorValue = 0.0f;
            var armorBuff = GetBuff(GameConst.Battle.ShieldBuffID);
            if (armorBuff != null)
            {
                var shield = armorBuff.LayerCount;
                if (truthDamage >= shield)
                {
                    reduceArmorValue = shield;
                }
                else
                {
                    reduceArmorValue = truthDamage;
                }
            }
            
            BuffChangeDamageDic.Clear();
            
            foreach (var buff in GetBuffList())
            {
                buff.ChangeDamageValue(BuffChangeDamageDic, paramModel);
            }

            foreach (var buff in target.GetBuffList())
            {
                buff.ChangeDamageValue(BuffChangeDamageDic, paramModel);
            }

            var changeDamageValue = BuffChangeDamageDic.Values.Sum();
            
            var damageValue = Math.Max(0, truthDamage * (1 - damageReducePct) - killDamageReduceInt - defendValue * (1 - armorPiercing) - reduceShieldValue - reduceArmorValue + changeDamageValue);
            return (truthDamage, reduceShieldValue, reduceArmorValue, damageValue);
        } 
        
        if (skillType == SkillType.ArtKilling)
        {
            var tech = GetProperty(BattlePropertyType.Tech);
            var skillDamageRateSum = damageRate;
            var skillDamageRateFloor = GetProperty(BattlePropertyType.SkillDamageRateFloor);
            var damageReducePct = target.GetProperty(BattlePropertyType.DamageReducePct);
            var killDamageReduceInt = target.GetProperty(BattlePropertyType.KillingDamageReduceInt);
            var breakValue = target.GetProperty(BattlePropertyType.Break);
            var truthDamage = Math.Max(0, tech * skillDamageRateSum * (1 + skillDamageRateFloor + skillDamageIncrease));
            var reduceShieldValue = 0.0f;
            var shieldBuff = GetBuff(GameConst.Battle.ShieldBuffID);
            if (shieldBuff != null)
            {
                var shield = shieldBuff.LayerCount;
                if (truthDamage >= shield)
                {
                    reduceShieldValue = shield;
                }
                else
                {
                    reduceShieldValue = truthDamage;
                }
            }

            var reduceArmorValue = 0.0f;
            var armorBuff = GetBuff(GameConst.Battle.ShieldBuffID);
            if (armorBuff != null)
            {
                var shield = armorBuff.LayerCount;
                if (truthDamage >= shield)
                {
                    reduceArmorValue = shield;
                }
                else
                {
                    reduceArmorValue = truthDamage;
                }
            }
             
            BuffChangeDamageDic.Clear();
            
            foreach (var buff in GetBuffList())
            {
                buff.ChangeDamageValue(BuffChangeDamageDic, paramModel);
            }

            foreach (var buff in target.GetBuffList())
            {
                buff.ChangeDamageValue(BuffChangeDamageDic, paramModel);
            }

            var changeDamageValue = BuffChangeDamageDic.Values.Sum();
            
            var damageValue = Math.Max(0, truthDamage * (1 - damageReducePct) - killDamageReduceInt - breakValue * (1 - armorPiercing) - reduceShieldValue - reduceArmorValue - changeDamageValue);
            return (truthDamage, reduceShieldValue, reduceArmorValue, damageValue);
        }

        return (1, 1, 0, 0);
    }

    public bool SkillIsKillingStyle()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return false;

        return skillBase.SkillIsKillingStyle();
    }

    #endregion

    #region Buff方法

    public int GetBuffCountByID(int buffID)
    {
        var buff = Buffs.TryGetValue(buffID);
        if (buff != null)
        {
            return buff.LayerCount;
        }

        return 0;
    }

    public List<BattleBuffBase> GetBuffList()
    {
        return Buffs.GetListValue();
    }

    public BattleBuffBase GetBuff(int buffID)
    {
        return Buffs.TryGetValue(buffID);
    }

    public bool HasBuff(int buffID)
    {
        return Buffs.GetListKey().Contains(buffID);
    }

    public bool HasBuffType(BuffType buffType)
    {
        return Buffs.GetListValue().Any(buff => buff.BuffType == buffType);
    }

    private void TriggerBuffBeAttack(float reduceHp, DamageType damageType, int attackID)
    {
        foreach (var buff in GetBuffList())
        {
            buff.BeAttack(reduceHp, damageType, attackID);
        }
    }

    public BattleBuffBase AddBuff(int buffID, BattleUnit spellCaster, int addCount, List<float> paramList = null)
    {
        var buff = Buffs.TryGetValue(buffID);
        if (buff == null)
        {
            buff = (BattleBuffBase)PoolManager.GetClass(BattleTypeManager.GetBuffType(buffID));
            buff.AddToUnit(buffID, this, spellCaster, addCount, paramList);
            Buffs.Add(buffID, buff);
            return buff;
        }
        else
        {
            var config = ConfigManager.GetBattleBuffConfig(buffID);
            if (config.OverlayType == (int)BuffOverlayType.Cover)
            {
                if (buff.LayerCount > addCount)
                {
                    return null;
                }
                buff.ClearLayerCount();
                var newBuff = PoolManager.GetClass<BattleBuffBase>();
                var buffConfig = ConfigManager.GetBattleBuffConfig(buffID);
                var limit = buffConfig.Limit;
                addCount = Math.Min(addCount, limit);
                newBuff.AddToUnit(buffID, this, spellCaster, addCount, paramList);
                return newBuff;
            }

            if (config.OverlayType == (int)BuffOverlayType.Overlap && !buff.IsMaxLayer())
            {
                buff.AddLayerCount(addCount);
                return buff;
            }

            return null;
        }
    }

    public void ReduceBuffLayerCount(int buffID, int reduceCount)
    {
        var buff = Buffs.TryGetValue(buffID);
        if (buff != null)
        {
            buff.ReduceLayerCount(reduceCount);
        }
    }

    /// <summary>
    /// 清空一个buffID 用这个方法
    /// </summary>
    /// <param name="buffID"></param>
    public bool ClearBuff(int buffID)
    {
        var buff = Buffs.TryGetValue(buffID);
        if (buff != null)
        {
            //固灾
            if (HasBuffMechanism(BuffMechanism.NotDirectRemoveAbnormalBuff) && buff.BuffType == BuffType.Abnormal)
            {
                return false;
            }
            buff.ClearLayerCount();
            return true;
        }

        return false;
    }
    /// <summary>
    /// 只做了buff列表移除
    /// </summary>
    /// <param name="buffID"></param>
    public void RemoveBuff(int buffID)
    {
        var buff = Buffs.TryGetValue(buffID);
        if (buff != null)
        {
            Buffs.Remove(buffID);
            PoolManager.RecycleClass(buff);
        }
    }

    public List<BattleBuffBase> GetRandomBuffByType(BuffType buffType, int count = 0)
    {
        var buffList = GetBuffList();
        if (buffType != BuffType.None)
        {
            buffList = buffList.Where(buff => buff.BuffType == buffType).ToList();
        }
        var weightList = Util.GetSameChanceList(buffList.Count);
        return Util.GetRandomNoSame(buffList, weightList, count);
    }

    public bool HasBuffMechanism(BuffMechanism mechanism)
    {
        return GetBuffList().Any(buff => buff.Config.Mechanism.Contains((int)mechanism));
    }
    
    #endregion

    public void Recycle()
    {
        PoolManager.RecycleClass(Property);
        PoolManager.RecycleClass(TakeSkillDataManager);
        PoolManager.RecycleClass(PreUseSkillDataManager);
        PoolManager.RecycleClass(UseSkillDataManager);
        
        foreach (var heartMethodBase in HeartMethods)
        {
            PoolManager.RecycleClass(heartMethodBase);
        }
        HeartMethods.Clear();

        foreach (var treasureBase in Treasures)
        {
            PoolManager.RecycleClass(treasureBase);
        }
        Treasures.Clear();
        while (SkillSequence.Any())
        {
            var skill = SkillSequence.Dequeue();
            PoolManager.RecycleClass(skill);
        }
        Variety.Clear();
        
        RoundBeDirectDamagedOpponentList.Clear();
        RoundBeDirectKillAttackOpponentList.Clear();
        IgnoreDirectKillingDamage = 0;
        StatusPersists = 0;
        ActionTimes = 0;
        RoundBeDirectDamageTimes = 0;
        RoundAlreadyActionTimes = 0;
        SpeedCounting = 0;
        ActionWheel = 0;
        ActionWheelOut = 0;
        BeCounter = false;
        DontBeCounter = 0;
        DontBeCounterByPowerKilling = 0;
        DontBeCounterByArtKilling = 0;
        IgnoreBeCounterByKeyTypeList.Clear();
        IgnoreBeCounterByDamage = 0;
        IgnoreTargetNotHasUpBuff = 0;
        IgnoreTargetNotHasDownBuff = 0;
        IgnoreTargetNotHasLeftBuff = 0;
        IgnoreTargetNotHasRightBuff = 0;
        IgnoreTargetSkillNotHasUpKey = 0;
        IgnoreTargetSkillNotHasDownKey = 0;
        IgnoreTargetSkillNotHasLeftKey = 0;
        IgnoreTargetSkillNotHasRightKey = 0;
        AccumulateDamageState = false;
        AccumulateDamageValue = 0;
        RoundBeDirectDamagedOpponentList.Clear();
        RoundBeDirectKillAttackOpponentList.Clear();
        IgnoreDirectKillingDamage = 0;
        StatusPersists = 0;
        GainStatusPersists = 0;
        KillUnitList.Clear();
        foreach (var model in PropDic.GetListValue())
        {
            PoolManager.RecycleClass(model);
        }
        PropDic.Clear();
    }
    
    /// <summary>
    /// 状态续存
    /// </summary>
    public int StatusPersists { get; private set; }
    public void AddStatusPersists(int state) => StatusPersists += state;
    /// <summary>
    /// 增益状态续存
    /// </summary>
    public int GainStatusPersists { get; private set; }
    public void AddGainStatusPersists(int state) => GainStatusPersists += state;
    
    public int NotBeAbnormalBuffEffect { get; set; }
    public void AddNotBeAbnormalBuffEffect (int state) => NotBeAbnormalBuffEffect += state;

    public void AddSkillClashState(bool clashState)
    {
        var skill = GetSkill();
        if (skill != null)
        {
            skill.AddClashState(clashState);
        }
    }

    public MinRecoverNaturalData AddMinRecoverNaturalData(int type, float value) => Property.AddMinRecoverNaturalData(type, value);
    public void RemoveMinRecoverNaturalData(int guid) => Property.RemoveMinRecoverNaturalData(guid);
    
    #region 个人道具
    
    private void InitTakeProp()
    {
        var takePropList = HeroData.GetTakeGameProp;
        foreach (var prop in takePropList)
        {
            AddProp(prop.ItemID, prop.Count);
        }
    }
    
    private DictAndList<int, BattleProp> PropDic = new();
    public List<BattleProp> GetUnitProp() => PropDic.GetListValue();
    public int ReduceProp(int itemID, int itemCount)
    {
        var propModel = PropDic.TryGetValue(itemID);
        if (propModel == null)
        {
            return 0;
        }

        if (propModel.Count > itemCount)
        {
            propModel.Count -= itemCount;
            return itemCount;
        }
        else
        {
            var reduceCount = propModel.Count;
            propModel.Count = 0;
            PropDic.Remove(itemID);
            PoolManager.RecycleClass(propModel);
            return reduceCount;
        }
    }

    public void AddProp(int itemID, int count)
    {
        var propModel = PropDic.TryGetValue(itemID);
        if (propModel == null)
        {
            propModel = PoolManager.GetClass<BattleProp>();
            propModel.ItemID = itemID;
            propModel.Count = count;
            PropDic.Add(itemID, propModel);
        }

        propModel.Count += count;
    }

    public int GetRandomProp()
    {
        if (PropDic.Count() <= 0)
            return 0;
        return Util.GetRandom(PropDic.GetListValue()).ItemID;
    }
    
    #endregion

    #region 重复招式数据

    public BattleRepeatUseSkill RepeatUseSkillData;

    private void TryRepeatUseSkill(BattleSkillBase skillBase, DamageParamModel paramModel = null)
    {
        var repeatData = skillBase.GetRepeatData(paramModel);
        if (repeatData == null) //如果没有下次就直接尝试移除
        {
            RemoveRepeatUseSkill();
            return;
        }
        if (!skillBase.IsRepeat)
        {
            if (RepeatUseSkillData == null)
            {
                var model = PoolManager.GetClass<BattleRepeatUseSkill>();
                model.SkillID = repeatData.SkillID;
                model.VariantID = repeatData.VariantID;
                model.TargetID = repeatData.TargetID;
                model.RepeatCount = 0;
                model.MaxRepeatCount = repeatData.MaxRepeatCount;
                model.IfLostChangeToOther = repeatData.IfLostChangeToOther;
                RepeatUseSkillData = model;
            }
            else
            {
                RemoveRepeatUseSkill();
            }
        }
        else if (RepeatUseSkillData != null)
        {
            if (Util.CombSkillGuid(RepeatUseSkillData.SkillID, RepeatUseSkillData.VariantID) == skillBase.SkillGuid)
            {
                RepeatUseSkillData.RepeatCount++;
                if (RepeatUseSkillData.RepeatCount >= RepeatUseSkillData.MaxRepeatCount)
                {
                    RemoveRepeatUseSkill();
                }
            }
        }
    }

    public void RemoveRepeatUseSkill()
    {
        if (RepeatUseSkillData != null)
        {
            PoolManager.RecycleClass(RepeatUseSkillData);
            RepeatUseSkillData = null;
        }
    }
    
    #endregion

    #region 状态覆盖

    public BattleUnitTransformState TransformState { get; private set; }

    public void SetTransformState(BattleUnitTransformState state)
    {
        TransformState = state;
    }
    

    #endregion
}
