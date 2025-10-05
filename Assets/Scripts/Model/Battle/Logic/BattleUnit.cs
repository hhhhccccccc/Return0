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

    public List<int> WearSkillList { get; set; }
    
    private Queue<BattleSkillBase> SkillSequence = new();

    #region 技能数据

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
    
    public void AddUseSkill(int skillID, BattleUnit target)
    {
        PreUseSkillDataManager.TryAddSkillPreUseData(skillID);
        var skillBase = (BattleSkillBase)PoolManager.GetClass(BattleTypeManager.GetSkillType(skillID));
        skillBase.Init(skillID, this, target);
        SkillSequence.Enqueue(skillBase);
    }

    public void TryRemoveUseSkill(SkillRemoveMomentType type)
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
                PreUseSkillDataManager.TryAddSkillPreUseDataBySkillEnd(skillBase.SkillID, type == SkillRemoveMomentType.BeCounter ? LastUseSkillState.BeCounter : LastUseSkillState.UseSuccess);
                UseSkillDataManager.AddUseSkillData(skillBase.SkillID, BattleLogicStateManager.Round, BattleLogicStateManager.ActionWheel);
                PoolManager.RecycleClass(skillBase);
            }
        }
    }
    
    #endregion
    
    public bool IsSelf { get; set; }
    public float ActionRadius { get; set; }
    public float ClashRadius { get; set; }
    public int Bgm { get; set; }

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
        Property.Init(heroData);
        PreUseSkillDataManager = PoolManager.GetClass<PreUseSkillDataManager>();
        UseSkillDataManager = PoolManager.GetClass<UseSkillDataManager>();
        ActionTimes = 0;
        RoundBeDirectDamageTimes = 0;
        RoundAlreadyActionTimes = 0;
        WearSkillList = HeroData.WearSkillList.Clone();
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
        Variety.AddRange(heroData.GetFightProperty_Variety());
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
            if (ReduceHp(AccumulateDamageValue, DamageType.InDirect))
            {
                
            }
        }
        AccumulateDamageValue = 0;

        if (BattleLogicStateManager.Round != 1)
        {
            RecoverGangQiNatural();
            RecoverXuanQiNatural();
            RecoverKeyNatural();
        }
    }

    private void RecoverKeyNatural() => Property.RecoverKeyNatural();

    /// <summary>
    /// 这一息结束
    /// </summary>
    public void OneActionWheelEnd()
    {
        ActionWheelOut = 0;
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
        
        //键有关
        IgnoreBeCounterByDamage = 0;
        IgnoreBeCounterByKeyTypeList.Clear();
        
        RoundBeDirectDamagedOpponentList.Clear();
        RoundBeDirectKillAttackOpponentList.Clear();
    }

    public bool IsAlive()
    {
        return GetProperty(BattlePropertyType.Hp) > 0;
    }
    
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

    protected virtual void Die()
    {
        
    }
    
    #region 属性
    
    public bool ChangeProperty(BattlePropertyType propType, float propValue, BattleSource source = BattleSource.None)
    {
        if (propType == BattlePropertyType.GangQiPct)
        {
            return Property.ChangeProperty(BattlePropertyType.GangQi,
                GetProperty(BattlePropertyType.MaxGangQi) * propValue, source);
        }
        
        if (propType == BattlePropertyType.XuanQiPct)
        {
            return Property.ChangeProperty(BattlePropertyType.XuanQi,
                GetProperty(BattlePropertyType.MaxXuanQi) * propValue, source);
        }
        
        if (propType == BattlePropertyType.GangQi && propValue > 0 && source == BattleSource.Skill)
        {
            propValue = Math.Max(propValue + GetProperty(BattlePropertyType.RecoverGangQiBySkillOffset), 0);
        }
        
        if (propType == BattlePropertyType.XuanQi && propValue > 0 && source == BattleSource.Skill)
        {
            propValue = Math.Max(propValue + GetProperty(BattlePropertyType.RecoverXuanQiBySkillOffset), 0);
        }
        
        return Property.ChangeProperty(propType, propValue, source);
    }

    public bool SetProperty(BattlePropertyType propType, float propValue, BattleSource source = BattleSource.None)
    {
        return Property.SetProperty(propType, propValue, source);
    }

    public float GetProperty(BattlePropertyType propType)
    {
        return Property.GetProperty(propType);
    }

    public float GetPropertyPct(BattlePropertyType propType)
    {
        return Property.GetPropertyPct(propType);
    }

    public void RemoveRandomKey(int count) => Property.RecoverRandomKey(count);
    
    public int ActionTimes { get; private set; }
    public int RoundBeDirectDamageTimes { get; private set; }
    public int RoundAlreadyActionTimes { get; private set; }
    
    public void EndAction()
    {
        RoundAlreadyActionTimes++;
        ActionTimes--;
        BeCounter = false;
    }

    public List<int> GetKeyList() => Property.GetKeyList();
    
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
    public void AddIgnoreBeCountByCount(int count) => IgnoreBeCounterByDamage += count;
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
    public bool CheckRoundBeDirectKillAttack(int attackID)
    {
        if (attackID == 0)
        {
            return RoundBeDirectKillAttackOpponentList.Count > 0;
        }
        
        return RoundBeDirectKillAttackOpponentList.Contains(attackID);
    }
    
    public virtual void BeDamage(ref DamageParamModel model)
    {
        var allDamage = model.HitDamageValue;
        if (model.HitDamageType == DamageType.Direct)
        {
            RoundBeDirectKillAttackOpponentList.Add(model.AttackID);
            
            //扣除甲  等量杀式扣除
            if (BattleUtil.SkillIsKillingStyle(model.HitSkillType))
            {
                foreach (var buff in GetBuffList())
                {
                    model.HitShieldValue += buff.ReduceArmor(ref allDamage);
                }
            }
            
            //扣除护盾
            foreach (var buff in GetBuffList())
            {
                model.HitShieldValue += buff.ReduceShield(ref allDamage);
            }

            //如果在累计伤害, 不算掉血
            if (AccumulateDamageState)
            {
                model.HitHpValue = 0;
                AccumulateDamageValue += allDamage;
            }
            else
            {
                model.HitHpValue = allDamage;
                if (model.HitHpValue > 0)
                {
                    RoundBeDirectDamagedOpponentList.Add(model.AttackID);
                    if (ReduceHp(model.HitHpValue, DamageType.Direct))
                    {

                    }
                }
            }
        }
        else if (model.HitDamageType == DamageType.InDirect)
        {
            model.HitHpValue = allDamage;
            if (model.HitHpValue > 0)
            {
                if (ReduceHp(model.HitHpValue, DamageType.InDirect))
                {

                }
            }
        }
    }

    /// <summary>
    /// 扣血
    /// </summary>
    /// <param name="reduceHp"></param>
    /// <param name="damageType"></param>
    /// <returns>是否死亡</returns>
    public virtual bool ReduceHp(float reduceHp, DamageType damageType)
    {
        //增加本回合受到直接伤害的次数
        if (damageType == DamageType.Direct && reduceHp > 0)
        {
            RoundBeDirectDamageTimes++;
        }
        ChangeProperty(BattlePropertyType.Hp, -reduceHp);
        var isDie = GetProperty(BattlePropertyType.Hp) <= 0;
        if (isDie)
        {
            Die();
        }
        return isDie;
    }
    
    #region 技能方法

    public float GetSkillDamageRate(SkillDataGetType getType, int skillID = 0, MomentParamModel paramModel = null)
    {
        switch (getType)
        {
            case SkillDataGetType.DamagePreview:
                if (skillID > 0)
                {
                    var damageBase = PreUseSkillDataManager.GetSkillPreUseDamage(skillID);
                    var skillType = BattleUtil.GetSkillTypeBySkillID(skillID);
                    var addValue = GetProperty(BattlePropertyType.TempSkillDamageAddValue);
                    var damageEffectDelta = PreUseSkillDataManager.GetSkillDamageEffectDelta(skillID);
                    switch (skillType)
                    {
                        case SkillType.None:
                            break;
                        case SkillType.PowerKilling:
                            addValue += GetProperty(BattlePropertyType.TempPowerSkillDamageAddValue);
                            break;
                        case SkillType.ArtKilling:
                            addValue += GetProperty(BattlePropertyType.TempArtSkillDamageAddValue);
                            break;
                        case SkillType.TechniqueImperialStyle:
                            break;
                        case SkillType.SpellFormula:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                
                    return damageBase + addValue * damageEffectDelta;
                }
                break;
            case SkillDataGetType.DamageBase:
                if (skillID > 0)
                {
                    return PreUseSkillDataManager.GetSkillPreUseDamage(skillID);
                }

                var skillBase = GetSkill();
                if (skillBase != null)
                {
                    return PreUseSkillDataManager.GetSkillPreUseDamage(skillBase.SkillID);
                }
                break;
            case SkillDataGetType.DamageCurr:
                var skill = GetSkill();
                if (skill != null)
                {
                    var damage = skill.GetSkillDamageRate;
                    var skillType = skill.GetSKillType;
                    var addValue = GetProperty(BattlePropertyType.TempSkillDamageAddValue);
                    var damageEffectDelta = skill.GetSkillDamageEffectDelta;
                    switch (skillType)
                    {
                        case SkillType.None:
                            break;
                        case SkillType.PowerKilling:
                            addValue += GetProperty(BattlePropertyType.TempPowerSkillDamageAddValue);
                            break;
                        case SkillType.ArtKilling:
                            addValue += GetProperty(BattlePropertyType.TempArtSkillDamageAddValue);
                            break;
                        case SkillType.TechniqueImperialStyle:
                            break;
                        case SkillType.SpellFormula:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                
                    return damage + (addValue + skill.GetSkillAttackAddWelly(paramModel)) * damageEffectDelta;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(getType), getType, null);
        }
        
        return 0;
    }

    public int GetSkillID()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return 0;

        return skillBase.GetSkillID();
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

    #endregion
   

    public void ForceSetSkill(int newSkillID)
    {
        //todo TryRemoveUseSkill();
        var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(EntityID);
        var target = BattleManager.GetUnit(behaviour.TargetID);
        //todo 判断目标是否合法
        behaviour.SkillID = newSkillID;
        AddUseSkill(newSkillID, target);
    }

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

    public int GetKey(BattleKeyType keyType) => Property.GetKey(keyType);
    public void SetKey(BattleKeyType keyType, int value) => Property.SetKey(keyType, value);
    public bool ChangeKey(BattleKeyType keyType, int value) => Property.ChangeKey(keyType, value);
    public int GetKeyCount() => Property.GetKeyCount();
    public int GetKeyMax() => Property.GetKeyMax();
    public void RemoveAllKey()
    {
        SetKey(BattleKeyType.KeyUp, 0);
        SetKey(BattleKeyType.KeyDown, 0);
        SetKey(BattleKeyType.KeyLeft, 0);
        SetKey(BattleKeyType.KeyRight, 0);
    }

    #endregion

    public float GetSkillGangQiCost(SkillDataGetType getType, int skillID = 0)
    {
        switch (getType)
        {
            case SkillDataGetType.CostPreview:
                if (skillID > 0)
                {
                    var cost = PreUseSkillDataManager.GetSkillPreUseGangQiCost(skillID);
                    return GetGangQiReduce(cost);
                }
                break;
            case SkillDataGetType.CheckCost:
                var skill = GetSkill();
                if (skill != null)
                {
                    var cost = skill.GetGangQiCost();
                    return GetGangQiReduce(cost);
                }
                break;
            case SkillDataGetType.ReleaseCost:
                skill = GetSkill();
                if (skill != null)
                {
                    return skill.GetGangQiCost();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(getType), getType, null);
        }

        return 0;
    }
    
    public float GetSkillXuanQiCost(SkillDataGetType getType, int skillID = 0)
    {
        switch (getType)
        {
            case SkillDataGetType.None:
                break;
            case SkillDataGetType.CostPreview:
                if (skillID > 0)
                {
                    var cost = PreUseSkillDataManager.GetSkillPreUseXuanQiCost(skillID);
                    return GetXuanQiReduce(cost);
                }
                break;
            case SkillDataGetType.CheckCost:
                var skill = GetSkill();
                if (skill != null)
                {
                    var cost = skill.GetXuanQiCost();
                    return GetXuanQiReduce(cost);
                }
                break;
            case SkillDataGetType.ReleaseCost:
                skill = GetSkill();
                if (skill != null)
                {
                    return skill.GetXuanQiCost();
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(getType), getType, null);
        }

        return 0;
    }

    public List<int> GetSkillKeyCost(SkillDataGetType getType, int skillID = 0)
    {
        switch (getType)
        {
            case SkillDataGetType.KeyPreview:
                if (skillID > 0)
                {
                    return PreUseSkillDataManager.GetSkillPreUseKeyCost(skillID);
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
    /// 预先检测能否释放成功
    /// </summary>
    /// <param name="skillID"></param>
    /// <returns></returns>
    public bool CheckSkillDoDesitionCostEnough(int skillID)
    {
        var hasGangQi = GetProperty(BattlePropertyType.GangQi);
        var costGangQi = GetSkillGangQiCost(SkillDataGetType.CostPreview, skillID);
        if (hasGangQi < costGangQi)
            return false;
        
        var hasXuanQi = GetProperty(BattlePropertyType.XuanQi);
        var costXuanQi = GetSkillXuanQiCost(SkillDataGetType.CostPreview, skillID);
        if (hasXuanQi < costXuanQi)
            return false;
        
        foreach (var (keyType, keyCount) in Util.KeyListToDictionary(GetSkillKeyCost(SkillDataGetType.KeyPreview, skillID)))
        {
            var hasKey = GetKey((BattleKeyType)keyType);
            if (hasKey < keyCount)
                return false;
        }
        
        return true;
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

        var hasGangQi = GetProperty(BattlePropertyType.GangQi);
        var costGangQi = GetSkillGangQiCost(SkillDataGetType.CheckCost);
        if (hasGangQi < costGangQi)
            return false;
        
        var hasXuanQi = GetProperty(BattlePropertyType.XuanQi);
        var costXuanQi = GetSkillXuanQiCost(SkillDataGetType.CheckCost);
        if (hasXuanQi < costXuanQi)
            return false;
        
        foreach (var (keyType, keyCount) in Util.KeyListToDictionary(GetSkillKeyCost(SkillDataGetType.CheckKey)))
        {
            var hasKey = GetKey((BattleKeyType)keyType);
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

        var gangQiCost = GetSkillGangQiCost(SkillDataGetType.ReleaseCost);
        ChangeProperty(BattlePropertyType.GangQi, -gangQiCost, BattleSource.Skill);
        var xuanQiCost = GetSkillXuanQiCost(SkillDataGetType.ReleaseCost);
        ChangeProperty(BattlePropertyType.XuanQi, -xuanQiCost, BattleSource.Skill);
        var keyCost = GetSkillKeyCost(SkillDataGetType.ReleaseKey);
        foreach (var (keyType, keyCount) in Util.KeyListToDictionary(keyCost))
        {
            ChangeKey((BattleKeyType)keyType, -keyCount);
        }

        return (gangQiCost, xuanQiCost, keyCost);
    }
    
    public float GetSkillDamageValue(BattleUnit target, DamageType damageType, BattleSource damageSource, float damageRate, DamageParamModel paramModel = null)
    {
        var skillType = GetSkillType();

        var skillBase = GetSkill();
        var skillDamageIncrease = 0.0f;
        if (skillBase != null)
        {
            skillDamageIncrease = skillBase.GetSkillAttackAddDamage(paramModel);
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
            return Math.Max(0, power * skillDamageRateSum * (1 + skillDamageRateFloor + skillDamageIncrease) * (1 - damageReducePct) - killDamageReduceInt - defendValue * (1 - armorPiercing));
        } 
        
        if  (skillType == SkillType.ArtKilling)
        {
            var tech = GetProperty(BattlePropertyType.Tech);
            var skillDamageRateSum = damageRate;
            var skillDamageRateFloor = GetProperty(BattlePropertyType.SkillDamageRateFloor);
            var damageReducePct = target.GetProperty(BattlePropertyType.DamageReducePct);
            var killDamageReduceInt = target.GetProperty(BattlePropertyType.KillingDamageReduceInt);
            var breakValue = target.GetProperty(BattlePropertyType.Break);
            return Math.Max(0, tech * skillDamageRateSum * (1 + skillDamageRateFloor + skillDamageIncrease) * (1 - damageReducePct) - killDamageReduceInt - breakValue * (1 - armorPiercing));
        }

        if (skillType == SkillType.TechniqueImperialStyle)
        {
            return 1;
        }

        return 0;
    }

    public bool SkillIsKillingStyle()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return false;

        return skillBase.SkillIsKillingStyle();
    }

    public bool CheckSkillCanUse(int skillID)
    {
        var skillConfig = ConfigManager.GetBattleSkillConfig(skillID);
        if (skillConfig.CheckSkillDoDesition.Count > 0)
        {
            if (skillConfig.CheckSkillDoDesitionRelation == 1 && skillConfig.CheckSkillDoDesition.All(conditionID =>
                    BattleMomentConditionManager.GetCondition(conditionID, this, skillID, null)))
            {
                return true;
            }

            if (skillConfig.CheckSkillDoDesitionRelation == 2 && skillConfig.CheckSkillDoDesition.Any(conditionID =>
                    BattleMomentConditionManager.GetCondition(conditionID, this, skillID, null)))
            {
                return true;
            }
        }
        
        return GetBuffList().All(buff => buff.CheckSkillCanUse(skillID)) && CheckSkillDoDesitionCostEnough(skillID);
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
                buff.ClearLayerCount();
                var newBuff = PoolManager.GetClass<BattleBuffBase>();
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

    public void ReduceBuff(int buffID, int reduceCount)
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
    public void ClearBuff(int buffID)
    {
        var buff = Buffs.TryGetValue(buffID);
        if (buff != null)
        {
            buff.ClearLayerCount();
        }
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

    public List<BattleBuffBase> GetRandomBuffByType(BuffType buffType, int count)
    {
        var buffList = GetBuffList();
        if (buffType != BuffType.None)
        {
            buffList = buffList.Where(buff => buff.BuffType == buffType).ToList();
        }
        var weightList = Util.GetSameChanceList(buffList.Count);
        return Util.GetRandomNoSame(buffList, weightList, count);
    }

    #endregion

    public void Recycle()
    {
        PoolManager.RecycleClass(Property);
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
    }
}
