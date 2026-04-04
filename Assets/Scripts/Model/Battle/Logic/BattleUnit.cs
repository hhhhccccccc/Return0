using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class ChangeActionWheelModel
{
    public int ActionWheel;
    public int ActionWheelOut;
}

public class BattleUnit : IModel, IRecycle
{
    #region Inject注入
    [Inject] private IPoolManager PM { get; set; }
    
    [Inject] private ILogManager LM { get; set; }
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
    public BattleMomentManager BattleMomentManager { get; set; }
    #region 技能数据
    public Queue<BattleSkillBase> SkillSequence = new();
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
            var skill = SkillSequence.Peek();
            return skill;
        }
        
        return null;
    }
    
    public void AddUseSkill(int skillGuid, BattleUnit target, bool needCostResource = true, bool isRepeat = false)
    {
        PreUseSkillDataManager.TryAddSkillPreUseData(skillGuid);
        var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
        var skillBase = (BattleSkillBase)PM.GetClass(BattleTypeManager.GetSkillType(skillID));
        skillBase.Init(skillGuid, this, target, needCostResource, isRepeat);
        SkillSequence.Enqueue(skillBase);
    }

    public void TryRemoveUseSkill(SkillRemoveMomentType type, DamageParamModel model = null)
    {
        if (SkillSequence.Any())
        {
            var skillBase = SkillSequence.Peek();
            if ((type == SkillRemoveMomentType.BeCounter) ||
                (skillBase.GetRemoveMomentList.Contains((int)type) && type == SkillRemoveMomentType.RoundEnd) ||
                (skillBase.GetRemoveMomentList.Contains((int)type) && type == SkillRemoveMomentType.AfterAction) ||
                (skillBase.GetRemoveMomentList.Contains((int)type) && type == SkillRemoveMomentType.BeforeNextAction && skillBase.CheckTriggerMoment(BattleMomentType.AfterAction)) ||
                (skillBase.GetRemoveMomentList.Contains((int)type) && type == SkillRemoveMomentType.NextRoundStart))
            {
                BattleMomentManager.TriggerSkillEnd(skillBase);
                PreUseSkillDataManager.TryAddSkillPreUseDataBySkillEnd(skillBase.SkillGuid, type == SkillRemoveMomentType.BeCounter ? LastUseSkillState.BeCounter : LastUseSkillState.UseSuccess);
                UseSkillDataManager.AddUseSkillData(skillBase.SkillGuid, BattleLogicStateManager.Round, BattleLogicStateManager.ActionWheel, skillBase.ClashState);
                AddRoundUsedSkillGuid(skillBase.SkillGuid);
                BattleLogicStateManager.AddRoundUsedSkillGuid(skillBase.SkillGuid);
                TryRepeatUseSkill(skillBase, model);
                SkillSequence.Dequeue();
                PM.RecycleClass(skillBase);
            }
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
    public BattleMomentViewType ViewType { get; set; }
    public virtual void Init(BattleField bf, HeroData heroData)
    {
        Bf = bf;
        IsSelf = bf.Uid == 1;
        HeroData = heroData;
        SlotIndex = heroData.SlotIndex;
        BattleManager.ResetUnitToDict(this);
        BattleMomentManager = PM.GetClass<BattleMomentManager>();
        Property = PM.GetClass<BattleProperty>();
        TakeSkillDataManager = PM.GetClass<TakeSkillDataManager>();
        PreUseSkillDataManager = PM.GetClass<PreUseSkillDataManager>();
        UseSkillDataManager = PM.GetClass<UseSkillDataManager>();
        TakeSkillDataManager.InitSkillData(heroData.WearSkillList);
        BattleMomentManager.Init(this, heroData);
        Property.Init(heroData, this);
        InitHasKey();
        
        InBreak = false;
        MaxPotionCount = 1;
        ActionTimes = 0;
        RoundBeDirectDamageTimes = 0;
        RoundAlreadyActionTimes = 0;
        TransformState = BattleUnitTransformState.None;
        
        InitHeartMethod();
        InitTreasure();
        ActionRadius = heroData.GetFightProperty_ActionRadius();
        ClashRadius = heroData.GetFightProperty_ClashRadius();
        Bgm = heroData.GetFightProperty_Bgm();
        Gr = heroData.GetJr();
        Variety.AddRange(heroData.GetFightProperty_Variety());
        InitTakeProp();
        BattleMomentManager.AfterUnitInit();
    }

    private void InitTreasure()
    {
        foreach (var treasureID in HeroData.WearTreasureList)
        {
            AddTreasure(treasureID);
        }
    }

    private void InitHeartMethod()
    {
        foreach (var heartMethodID in HeroData.WearHeartMethodList)
        {
            AddHeartMethod(heartMethodID);
        }
    }

    private void InitHasKey()
    {
        if (BattleMomentManager.CheckHasMethod(GameConst.Battle.HeartMethod10090))
        {
            var list = new List<BattleKeyType>
            {
                BattleKeyType.KeyUp,
                BattleKeyType.KeyUp,
                BattleKeyType.KeyDown,
                BattleKeyType.KeyDown,
                BattleKeyType.KeyLeft,
                BattleKeyType.KeyLeft,
                BattleKeyType.KeyRight,
                BattleKeyType.KeyRight,
            };
            ChangeKeyList(list, true, ChangeKeyReason.Init);
            return;
        }
        
        //AddRandomKey(Property.GetKeyProperty(BattleKeyType.KeyMax) + GetKeyProperty(BattleKeyType.KeyMaxEx), ChangeKeyReason.Init);
        AddRandomKey(50, ChangeKeyReason.Init);
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

        if (BattleLogicStateManager.Round != 1)
        {
            if (NotRecoverGangQiNatural <= 0)
            {
                RecoverGangQiNatural();
            }

            if (NotRecoverXuanQiNatural <= 0)
            {
                RecoverXuanQiNatural();
            }
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
        IgnoreBeCounterByKeyTypeList.Clear();
        
        RoundBeDirectDamagedOpponentList.Clear();
        RoundBeDirectKillAttackOpponentList.Clear();
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
        
        #region 战斗资源特殊计算

        if (propType == BattlePropertyType.GangQi)
        {
            if (finalPropValue > 0)
            {
                finalPropValue = GetGangQiRecover(finalPropValue);
            }
            else if (finalPropValue < 0)
            {
                finalPropValue = GetGangQiReduce(finalPropValue);
            }
        }   
        
        if (propType == BattlePropertyType.XuanQi)
        {
            if (finalPropValue > 0)
            {
                finalPropValue = GetXuanQiRecover(finalPropValue);
            }
            else if (finalPropValue < 0)
            {
                finalPropValue = GetXuanQiReduce(finalPropValue);
            }
        }   

        #endregion
        
      
        BattleMomentManager.BeforeChangeProperty(propType, ref finalPropValue, source);
        if (propType == BattlePropertyType.GangQi && finalPropValue > 0 && source == BattleSource.Skill)
        {
            finalPropValue = Math.Max(finalPropValue + GetProperty(BattlePropertyType.RecoverGangQiBySkillOffset), 0);
        }
        
        if (propType == BattlePropertyType.XuanQi && finalPropValue > 0 && source == BattleSource.Skill)
        {
            finalPropValue = Math.Max(finalPropValue + GetProperty(BattlePropertyType.RecoverXuanQiBySkillOffset), 0);
        }
        
        finalPropValue = Property.ChangeProperty(propType, finalPropValue, source);
        BattleMomentManager.AfterChangeProperty(propType, originPropValue, finalPropValue, source);

        var model = PM.GetClass<UnitChangePropertyEventModel>();
        model.UnitID = EntityID;
        model.PropType = propType;
        model.PropValue = finalPropValue;
        model.Source = source;
        MessageManager.DispatchMsg(model);
        PM.RecycleClass(model);
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

    public float GetProperty(BattlePropertyType propType, GetPropertySourceModel model = null)
    {
        //彻buff单独写
        if (propType == BattlePropertyType.Power || propType == BattlePropertyType.Tech)
        {
            var buff = GetBuff(GameConst.Battle.BuffChe);
            if (buff != null)
            {
                return buff.GetProperty(propType);
            }
        }
        
        return Property.GetProperty(propType, model);
    }

    public float GetPropertyPct(BattlePropertyType propType)
    {
        return Property.GetPropertyPct(propType);
    }
    
    public void ForceRefreshPropertyLimit() => Property.TryAdjustLimit();

    private bool InBreak { get; set; }
    public bool IsAlive() => GetProperty(BattlePropertyType.Hp) > 0 && !InBreak;
    
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
    /// <summary>
    /// 行动是否被揭示
    /// </summary>
    public bool IsBeActionReveals { get; set; }

    public bool GetIsBeActionReveals()
    {
        if (BattleMomentManager.CheckHasMethod(GameConst.Battle.HeartMethod10091))
        {
            return true;
        }
        
        if (BattleMomentManager.CheckHasMethod(GameConst.Battle.HeartMethod10116))
        {
            var has = false;
            foreach (var unit in BattleManager.GetAllOpponentUnit(EntityID, true))
            {
                var skill = unit.GetSkill();
                if (skill != null && skill.Target == this)
                {
                    has = true;
                    break;
                }
            }

            if (!has)
            {
                return false;
            }
        }

        if (BattleMomentManager.CheckHasMethod(GameConst.Battle.HeartMethod10153))
        {
            var skill = GetSkill();
            if (skill != null)
            {
                var costKey = skill.GetKeyCostList;
                if (costKey.Distinct().Count() >= 3)
                {
                    return false;
                }
            }
        }
        
        return IsBeActionReveals;
    }
    
    /// <summary>
    /// 上次行动距离现在多远
    /// </summary>
    public int LastActionWheelToNow { get; private set; }
    public void AddLastActionWheelToNow(int actionWheel)
    {
        LastActionWheelToNow += actionWheel;
    }
    
    public void EndAction()
    {
        PreChangeActionWheel = 0;
        LastActionWheelToNow = 0;
        RoundAlreadyActionTimes++;
        ActionTimes--;
        BeCounter = false;
        ActionWheelIsAction = true;
        BattleMomentManager.EndAction();

        var model = PM.GetClass<UnitTriggerEndActionEventModel>();
        model.EntityID = EntityID;
        MessageManager.DispatchMsg(model);
        PM.RecycleClass(model);
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

    private int NotRecoverGangQiNatural { get; set; }
    public void AddNotRecoverGangQiNatural(int state) => NotRecoverGangQiNatural += state;
    private int NotRecoverXuanQiNatural { get; set; }
    public void AddNotRecoverXuanQiNatural(int state) => NotRecoverXuanQiNatural += state;
    
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
    /// 不会被破招的键的列表
    /// </summary>
    private List<BattleKeyType> IgnoreBeCounterByKeyTypeList = new();
    public void AddIgnoreBeCounterKey(BattleKeyType key) => IgnoreBeCounterByKeyTypeList.Add(key);
    /// <summary>
    /// 尝试被破招
    /// </summary>
    public bool TryBeCounter(int attackerID, MomentParamModel model)
    {
        var attack = BattleManager.GetUnit(attackerID);
        var attackSkill = attack.GetSkill();
        var costKey = attackSkill.GetKeyCostList;
        
        if (IgnoreBeCounterByKeyTypeList.Any(hasKey => costKey.Contains((int)hasKey)))
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

        if (BattleMomentManager.CheckDontBeCounter(model))
        {
            return false;
        }
        
        BeCounter = true;
        BattleMomentManager.BeCounter();
        return true;
    }
    
    //改变息  是否是预先计算
    public int PreChangeActionWheel { get; set; }
    public ChangeActionWheelModel ChangeActionWheel(int value, bool isPre = false)
    {
        var model = new ChangeActionWheelModel();
        
        if (isPre)
        {
            PreChangeActionWheel = value;
        }
        
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

        model.ActionWheel = ActionWheel;
        model.ActionWheelOut = ActionWheelOut;
        return model;
    }

    /// <summary>
    /// 延迟受伤
    /// </summary>
    private bool AccumulateDamageState { get; set; }
    public void SetAccumulateDamage() => AccumulateDamageState = true;
    private float AccumulateDamageValue { get; set; }
    
    /// <summary>
    /// 本回合对自己造成过直接伤害的对手ID
    /// </summary>
    private List<int> RoundBeDirectDamagedOpponentList = new();
    public bool CheckRoundBeSameDirectDamaged(int attackID) => RoundBeDirectDamagedOpponentList.Contains(attackID);
    
    /// <summary>
    /// 本回合对自己使用过直接杀式攻击的对手ID
    /// </summary>
    private List<int> RoundBeDirectKillAttackOpponentList = new();
    
    /// <summary>
    /// 本回合使用过的技能
    /// </summary>
    public List<int> RoundUsedSkillGuid = new();
    public void AddRoundUsedSkillGuid(int skillGuid) => RoundUsedSkillGuid.Add(skillGuid);
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
        var attackID = model.GetOtherID(EntityID);
        var damageType = model.GetSelfDamageType(attackID);
        if (damageType == DamageType.Direct)
        {
            if (BattleMomentManager.IgnoreSkillDirectDamage(model))
            {
                model.SetAttackTruthDamageValue(attackID, 0);
                model.SetAttackHpValue(attackID, 0);
                model.SetAttackShieldValue(attackID, 0);
                model.SetAttackArmorValue(attackID, 0);
            }
            
            RoundBeDirectKillAttackOpponentList.Add(attackID);

            var attacker = BattleManager.GetUnit(attackID);
            var attackerSkill = attacker.GetSkill();
            var truthDamageValue = 0.0f;
            if (attackerSkill != null)
            {
                if (attackerSkill.IsTrueDamage(model))
                {
                    truthDamageValue = model.GetSelfAttackTruthDamageValue(attackID);
                }
                else
                {
                    truthDamageValue = model.GetSelfAttackHpValue(attackID);
                    if (model.GetSelfAttackShieldValue(attackID) > 0)
                    {
                        ReduceBuffLayerCount(GameConst.Battle.ShieldBuffID, model.GetSelfAttackShieldValue(attackID).ToInt());
                    }
                
                    if (model.GetSelfAttackArmorValue(attackID) > 0)
                    {
                        ReduceBuffLayerCount(GameConst.Battle.ArmorBuffID, model.GetSelfAttackArmorValue(attackID).ToInt());
                    }
                }
            }
            
            //如果在累计伤害, 不算掉血
            if (AccumulateDamageState)
            {
                AccumulateDamageValue += truthDamageValue;
            }
            else
            {
                RoundBeDamageValue += truthDamageValue;
                if (truthDamageValue > 0)
                {
                    RoundBeDirectDamagedOpponentList.Add(attackID);
                    if (ReduceHp(truthDamageValue, DamageType.Direct, attackID, source: BattleSource.Skill, isReduceHpMax: model.GetOtherDamageReduceMaxHp(EntityID))) 
                    {
                        
                    }
                }
            }
            
            BattleMomentManager.BeDamage(damageType);
        }
        else if (model.GetSelfDamageType(attackID) == DamageType.InDirect)
        {
            var attacker = BattleManager.GetUnit(attackID);
            var attackerSkill = attacker.GetSkill();
            var damageValue = 0.0f;
            if (attackerSkill != null)
            {
                damageValue = attackerSkill.IsTrueDamage(model) ? model.GetSelfAttackTruthDamageValue(attackID) : model.GetSelfAttackHpValue(attackID);
            }
            RoundBeDamageValue += damageValue;
            if (damageValue > 0)
            {
                if (ReduceHp(damageValue, DamageType.InDirect, attackID, source: BattleSource.Skill))
                {
                    
                }
            }
        }
    }
    
    /// <summary>
    /// 击杀的列表ID
    /// </summary>
    public List<int> KillUnitList = new();

    private void AddKillID(int beKillID)
    {
        KillUnitList.Add(beKillID);
        BattleMomentManager.OnKillUnit(beKillID);
        BattleLogicStateManager.AddRoundUnitDieList(beKillID);
    }

    public virtual void SetHp(float setValue, int setID, BattleSource source = BattleSource.None)
    {
        SetProperty(BattlePropertyType.Hp, setValue, source);
        var isDie = GetProperty(BattlePropertyType.Hp) <= 0;
        if (isDie)
        {
            var attack = BattleManager.GetUnit(setID);
            attack.AddKillID(EntityID);
            Die();
        }
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
        BattleMomentManager.BeforeReduceHp(false, healValue, DamageType.None, EntityID, false);
        var value = ChangeProperty(BattlePropertyType.Hp, healValue, source);
        BattleMomentManager.AfterReduceHp(false, healValue, DamageType.None, EntityID, false);
        return value;
    }

    /// <summary>
    /// 扣血
    /// </summary>
    /// <param name="reduceHp"></param>
    /// <param name="damageType"></param>
    /// <param name="attackID"></param>
    /// <param name="triggerBeHitEventModel"></param>
    /// <param name="source"></param>
    /// <param name="isReduceHpMax"></param>
    /// <returns></returns>
    public virtual bool ReduceHp(float reduceHp, DamageType damageType, int attackID, bool triggerBeHitEventModel = true, BattleSource source = BattleSource.None, bool isReduceHpMax = false)
    {
        var attacker = BattleManager.GetUnit(attackID);
        
        if (isReduceHpMax)
        {
            ChangeProperty(BattlePropertyType.MaxHpInt, -reduceHp, source);
        }
        else
        {
            //增加本回合受到直接伤害的次数
            if (damageType == DamageType.Direct && reduceHp > 0)
            {
                RoundBeDirectDamageTimes++;
            }
            ChangeProperty(BattlePropertyType.Hp, -reduceHp, source);
        }
        
        var isDie = GetProperty(BattlePropertyType.Hp) <= 0;
        if (isDie)
        {
            var attack = BattleManager.GetUnit(attackID);
            attack.AddKillID(EntityID);
            Die();
        }
        BattleMomentManager.AfterReduceHp(true, reduceHp, damageType, attackID, isReduceHpMax);
        if (triggerBeHitEventModel)
        {
            TriggerReduceHpEventModel(reduceHp, damageType, attackID);
        }
        //是否要清理所有buff
        
        return isDie;
    }

    protected virtual void Die()
    {
        SetBreak(true);
    }

    public void SetBreak(bool state)
    {
        InBreak = state;
        if (state)
        {
            var model = PM.GetClass<UnitDieEventModel>();
            model.DieID = EntityID;
            MessageManager.DispatchMsg(model);
            PM.RecycleClass(model);
        }
    }

    /// <summary>
    /// 一般会触发直接关系的 像铁索连环加在对方身上 自己受伤时获取不到就用事件处理
    /// </summary>
    /// <param name="reduceHp"></param>
    /// <param name="damageType"></param>
    /// <param name="attackID"></param>
    private void TriggerReduceHpEventModel(float reduceHp, DamageType damageType, int attackID)
    {
        var model = PM.GetClass<UnitBeHitEventModel>();
        model.HitID = EntityID;
        model.DamageValue = reduceHp;
        model.DamageType = damageType;
        model.AttackID = attackID;
        MessageManager.DispatchMsg(model);
        PM.RecycleClass(model);
    }
    
    #region 技能方法

    public float GetSkillDamageWelly(SkillDataGetType getType, int skillGuid = 0, MomentParamModel paramModel = null)
    {
        switch (getType)
        {
            case SkillDataGetType.WellyRatePreview:
                if (skillGuid > 0)
                {
                    var (skillID, variantID) = Util.UnCombSkillGuid(skillGuid);
                    var wellyRateBase = PreUseSkillDataManager.GetSkillPreUseWellyRateBase(skillGuid);
                    BattleMomentManager.TrySetBaseWellyRate(skillGuid, ref wellyRateBase);
                    var skillType = BattleUtil.GetSkillTypeBySkillID(skillID);
                    var tempWellyRateEx = 0.0f;
                    var momentWellyRateExSum = BattleMomentManager.GetWellyRateExSum(skillGuid);
                    var momentWellyIncreaseSum = BattleMomentManager.GetWellyIncreaseSum(skillGuid);
                    switch (skillType)
                    {
                        case SkillType.None:
                            break;
                        case SkillType.PowerKilling:
                            tempWellyRateEx = GetProperty(BattlePropertyType.TempPowerSkillWellyRateEx);
                            break;
                        case SkillType.ArtKilling:
                            tempWellyRateEx = GetProperty(BattlePropertyType.TempArtSkillWellyRateEx);
                            break;
                        case SkillType.TechniqueImperialStyle:
                            break;
                        case SkillType.SpellFormula:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                    
                    var wellyRateEx = (tempWellyRateEx + momentWellyRateExSum) * (1 + momentWellyIncreaseSum);
                    BattleMomentManager.TrySetWellyRateEx(skillGuid, ref wellyRateEx);
                    return wellyRateBase + wellyRateEx;
                }
                break;
            case SkillDataGetType.WellyRateBase:
                if (skillGuid > 0)
                {
                    var wellyRate = PreUseSkillDataManager.GetSkillPreUseWellyRateBase(skillGuid);
                    BattleMomentManager.TrySetBaseWellyRate(skillGuid, ref wellyRate);
                    return wellyRate;
                }

                var skillBase = GetSkill();
                if (skillBase != null)
                {
                    var wellyRate = skillBase.GetWellyRateBase(paramModel);
                    BattleMomentManager.TrySetBaseWellyRate(skillGuid, ref wellyRate);
                    return wellyRate;
                }
                break;
            case SkillDataGetType.WellyRateCurr:
                var skill = GetSkill();
                if (skill != null)
                {
                    var wellyRateBase = skill.GetWellyRateBase(paramModel);
                    BattleMomentManager.TrySetBaseWellyRate(skill.SkillGuid, ref wellyRateBase);
                    var skillType = skill.GetSKillType;
                    var tempWellyRateEx = 0.0f;
                    var momentWellyRateExSum = BattleMomentManager.GetWellyRateExSum(skill.SkillGuid);
                    var momentWellyIncreaseSum = BattleMomentManager.GetWellyIncreaseSum(skill.SkillGuid);
                    switch (skillType)
                    {
                        case SkillType.None:
                            break;
                        case SkillType.PowerKilling:
                            tempWellyRateEx = GetProperty(BattlePropertyType.TempPowerSkillWellyRateEx);
                            break;
                        case SkillType.ArtKilling:
                            tempWellyRateEx = GetProperty(BattlePropertyType.TempArtSkillWellyRateEx);
                            break;
                        case SkillType.TechniqueImperialStyle:
                            break;
                        case SkillType.SpellFormula:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                
                    var wellyRateEx = (tempWellyRateEx + momentWellyRateExSum) * (1 + momentWellyIncreaseSum);
                    BattleMomentManager.TrySetWellyRateEx(skill.SkillGuid, ref wellyRateEx);
                    
                    return wellyRateBase + wellyRateEx;
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

    public int AddActionTimes(int times)
    {
        if (times < 0 && ActionTimes <= 0)
        {
            return 0;
        }
        ActionTimes += times;
        return ActionTimes;
    }

    #region 键相关

    public List<int> GetAllKeyTypeList() => Property.GetAllKeyTypeList();
    public int GetKeyCount(BattleKeyType keyType, bool isLocked = false) => Property.GetKeyCount(keyType, isLocked);
    
    public List<BattleKey> AddRandomKey(int count, ChangeKeyReason reason, ChangeKeyType changeType = ChangeKeyType.None)
    {
        //失重  无法通过状态、招式、心法效果获得键
        if (HasBuff(GameConst.Battle.Buff90019))
        {
            if (reason == ChangeKeyReason.SkillEffect || reason == ChangeKeyReason.BuffEffect ||
                reason == ChangeKeyReason.HeartMethodEffect)
            {
                return null;
            }
        }
        
        var getKey = Util.GetRandomKey(count);
        return ChangeKeyList(getKey, true, reason, changeType);
    }
    public int GetKeyProperty(BattleKeyType keyType) => Property.GetKeyProperty(keyType);
    public int ChangeKeyProperty(BattleKeyType keyType, int count, ChangeKeyReason reason = ChangeKeyReason.None) => Property.ChangeKeyProperty(keyType, count, reason);
    public List<BattleKey> RemoveRandomKey(int count, ChangeKeyReason reason = ChangeKeyReason.None, ChangeKeyType changeType = ChangeKeyType.None)
    {
        var allKey = GetAllKeyTypeList().Clone();
        var removeList = Util.GetRandomNoSame(allKey, Util.GetSameChanceList(allKey.Count), count);
        var result = ChangeKeyList(removeList.Select(o => (BattleKeyType)o).ToList(), false, reason, changeType);
        return result;
    }
    
    public List<BattleKey> ChangeKeyList(List<BattleKeyType> keyTypeList, bool isAdd, ChangeKeyReason reason = ChangeKeyReason.None, ChangeKeyType changeType = ChangeKeyType.None)
    {
        var list = new List<BattleKey>();
        var dict = Util.KeyListToDictionary(keyTypeList);
        foreach (var (keyType, keyCount) in dict)
        {
            list.AddRange(ChangeKey((BattleKeyType)keyType, isAdd ? keyCount : -keyCount, reason, changeType)); 
        }
        
        BattleMomentManager.AfterChangeKey(list, isAdd, reason, changeType);
        return list;
    }
    
    /// <summary>
    /// 检查是否有溢出的键 有的话返回溢出的键
    /// </summary>
    /// <returns></returns>
    public List<BattleKey> CheckKeyLimit() => Property.CheckKeyLimit();
    
    /// <summary>
    /// 获取改变了哪些键
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="value"></param>
    /// <param name="reason"></param>
    /// <param name="changeType"></param>
    /// <returns></returns>
    private List<BattleKey> ChangeKey(BattleKeyType keyType, int value, ChangeKeyReason reason = ChangeKeyReason.None, ChangeKeyType changeType = ChangeKeyType.None)
    {
        BattleMomentManager.ConvertChangeKey(ref keyType, value);
        var changeKeyList = Property.ChangeKey(keyType, value, reason);
        if (value > 0)
        {
            BattleMomentManager.KeyAdd(keyType, changeKeyList, reason, changeType);
        }
        else
        {
            BattleMomentManager.KeyReduce(keyType, changeKeyList, reason, changeType);
        }
        
        var model = PM.GetClass<UnitChangeKeyEventModel>();
        model.UnitID = EntityID;
        model.KeyType = keyType;
        model.Count = value;
        model.Reason = reason;
        model.ChangeType = changeType;
        MessageManager.DispatchMsg(model);
        PM.RecycleClass(model);
        return changeKeyList;
    }
    
    public List<BattleKey> AddBattleKey(BattleKey key, ChangeKeyReason reason = ChangeKeyReason.None, ChangeKeyType changeType = ChangeKeyType.None)
    {
        var changeKeyList = Property.AddBattleKey(key, reason, changeType);
        BattleMomentManager.KeyAdd(key.KeyType, changeKeyList, reason, changeType);
        var model = PM.GetClass<UnitChangeKeyEventModel>();
        model.UnitID = EntityID;
        model.KeyType = key.KeyType;
        model.Count = 1;
        model.Reason = reason;
        model.ChangeType = changeType;
        MessageManager.DispatchMsg(model);
        PM.RecycleClass(model);
        return changeKeyList;
    }

    public int GetAllKeyCount(bool isPreCalculateActionWheel = false)
    {
        if (isPreCalculateActionWheel)
        {
            var treasure = BattleMomentManager.GetTreasureByFeature(TreasureFeature.DuMengZhou);
            if (treasure != null)
            {
                return treasure.GetConfigParamInt(0);
            }
        }
        
        return Property.GetAllKeyCount();
    }
    public int GetKeyPropertyMax() => Property.GetKeyPropertyMax();
    public List<BattleKey> RemoveAllKey(ChangeKeyReason reason = ChangeKeyReason.None,
        ChangeKeyType changeType = ChangeKeyType.None)
    {
        var allKey = GetAllKeyTypeList().Select(o => (BattleKeyType)o).ToList();
        return ChangeKeyList(allKey, false, reason, changeType);
    }
    public List<BattleKey> LockRandomKey(int count) => Property.LockRandomKey(count);
    public BattleKey UnlockKey(int guid) => Property.UnlockKey(guid);
    public List<BattleKey> PollutionRandomKey(int count) => Property.PollutionRandomKey(count);
    public BattleKey UnPollutionKey(int guid) => Property.UnPollutionKey(guid);
    private void RecoverKeyNatural()
    {
        if (BattleMomentManager.CheckHasMethod(GameConst.Battle.HeartMethod10090))
        {
            var list = new List<BattleKeyType>
            {
                BattleKeyType.KeyUp,
                BattleKeyType.KeyDown,
                BattleKeyType.KeyLeft,
                BattleKeyType.KeyRight,
            };
            ChangeKeyList(list, false, ChangeKeyReason.NaturalRecover);
            return;
        }
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
                    (gangQiCost, xuanQiCost) = BattleMomentManager.ChangeResourceCost(gangQiCost, xuanQiCost);
                    var model = PM.GetClass<GetPropertySourceModel>();
                    model.SourceType = GetPropertySourceType.GetSkillCostView;
                    model.TypeID = skillGuid;
                    (gangQiCost, xuanQiCost) = (GetGangQiReduce(gangQiCost, model), GetXuanQiReduce(xuanQiCost, model));
                    PM.RecycleClass(model);
                    return (gangQiCost, xuanQiCost);
                }
                break;
            case SkillDataGetType.CheckCost:
                var skill = GetSkill();
                if (skill != null)
                {
                    var gangQiCost = skill.GetGangQiCost();
                    var xuanQiCost = skill.GetXuanQiCost();
                    var model = PM.GetClass<GetPropertySourceModel>();
                    model.SourceType = GetPropertySourceType.GetSkillCostCheck;
                    model.TypeID = skill.SkillGuid;
                    (gangQiCost, xuanQiCost) = (GetGangQiReduce(gangQiCost, model), GetXuanQiReduce(xuanQiCost, model));
                    PM.RecycleClass(model);
                    return (gangQiCost, xuanQiCost);
                }
                break;
            case SkillDataGetType.ReleaseCost:
                skill = GetSkill();
                if (skill != null)
                {
                    var gangQiCost = skill.GetGangQiCost();
                    var xuanQiCost = skill.GetXuanQiCost();
                    var model = PM.GetClass<GetPropertySourceModel>();
                    model.SourceType = GetPropertySourceType.GetSkillCostLogic;
                    model.TypeID = skill.SkillGuid;
                    (gangQiCost, xuanQiCost) = (GetGangQiReduce(gangQiCost, model), GetXuanQiReduce(xuanQiCost, model));
                    PM.RecycleClass(model);
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
    public float GetGangQiReduce(float propValue, GetPropertySourceModel model = null) => Property.GetGangQiReduce(propValue, model);
    public float GetXuanQiRecover(float propValue) => Property.GetXuanQiRecover(propValue);
    public float GetXuanQiReduce(float propValue, GetPropertySourceModel model = null) => Property.GetXuanQiReduce(propValue, model);
    
    
    #endregion

    #region 技能方法

    private List<int> ReplaceKeyList = new();
    
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

        var keyCostList = GetSkillKeyCost(SkillDataGetType.KeyPreview, skillGuid);
        foreach (var (keyType, keyCount) in Util.KeyListToDictionary(keyCostList))
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
            
            ReplaceKeyList.Clear();
            ReplaceKeyList.Add(keyType);
            BattleMomentManager.KeyReplace(ReplaceKeyList, (BattleKeyType)keyType);
            var costKeyCount = keyCostList.Count(kt => ReplaceKeyList.Contains(kt));
            var hasKeyCount = ReplaceKeyList.Sum(kt => GetKeyCount((BattleKeyType)kt));
            if (costKeyCount < hasKeyCount)
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
        {
            LM.D($"{EntityID} 没有技能");
            return false;
        }

        var (costGangQi, costXuanQi) = GetSkillQiCost(SkillDataGetType.CheckCost);
        
        var hasGangQi = GetProperty(BattlePropertyType.GangQi);
        var gangQiDelta = hasGangQi - costGangQi;
        if (hasGangQi < costGangQi)
        {
            var replaceGangQiCost = BattleMomentManager.GetReplaceSkillGangQiCost();
            if (gangQiDelta + replaceGangQiCost < 0)
            {
                LM.D($"{EntityID} 刚气不足， 拥有{hasGangQi}，需要{costGangQi}");
                return false;
            }
        }
           
        
        var hasXuanQi = GetProperty(BattlePropertyType.XuanQi);
        var xuanQiDelta = hasXuanQi - costGangQi;
        if (hasXuanQi < costXuanQi)
        {
            var replaceXuanQiCost = BattleMomentManager.GetReplaceSkillXuanQiCost();
            if (xuanQiDelta + replaceXuanQiCost < 0)
            {
                LM.D($"{EntityID} 玄气不足， 拥有{hasXuanQi}，需要{costXuanQi}");
                return false;
            }
        }

        var keyCostList = GetSkillKeyCost(SkillDataGetType.CheckKey);
        foreach (var (keyType, keyCount) in Util.KeyListToDictionary(keyCostList))
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
            
            ReplaceKeyList.Clear();
            ReplaceKeyList.Add(keyType);
            BattleMomentManager.KeyReplace(ReplaceKeyList, (BattleKeyType)keyType);
            var costKeyCount = keyCostList.Count(kt => ReplaceKeyList.Contains(kt));
            var hasKeyCount = ReplaceKeyList.Sum(kt => GetKeyCount((BattleKeyType)kt));
            if (hasKeyCount < costKeyCount)
            {
                LM.D($"{EntityID} 键不足 {keyType}， 拥有{hasKeyCount}，需要{costKeyCount}");
                return false;
            }
        }
        
        return true;
    }
    
    /// <summary>
    /// 消耗技能的资源
    /// </summary>
    public (float, float, List<BattleKey>) CostSkillNeedResource()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
        {
            return (0, 0, new List<BattleKey>());
        }

        if (!skillBase.NeedCostResource)
        {
            return (0, 0, new List<BattleKey>());
        }

        var (gangQiCost, xuanQiCost) = GetSkillQiCost(SkillDataGetType.ReleaseCost);
        var hasGangQi = GetProperty(BattlePropertyType.GangQi);
        if (hasGangQi < gangQiCost)
        {
            gangQiCost -= hasGangQi;
            BattleMomentManager.EffectReplaceSkillGangQiCost(ref gangQiCost);
        }
        ChangeProperty(BattlePropertyType.GangQi, -gangQiCost, BattleSource.Skill);
        var hasXuanQi = GetProperty(BattlePropertyType.XuanQi);
        if (hasXuanQi < xuanQiCost)
        {
            xuanQiCost -= hasXuanQi;
            BattleMomentManager.EffectReplaceSkillXuanQiCost(ref xuanQiCost);
        }
        ChangeProperty(BattlePropertyType.XuanQi, -xuanQiCost, BattleSource.Skill);
        var keyCost = GetSkillKeyCost(SkillDataGetType.ReleaseKey);
        var changeKeyList = ChangeKeyList(keyCost.Select(o =>(BattleKeyType)o).ToList(), false, ChangeKeyReason.SkillCost, ChangeKeyType.Cost);
        skillBase.SetTruthSkillCost(gangQiCost, xuanQiCost, changeKeyList);
        return (gangQiCost, xuanQiCost, changeKeyList);
    }

    /// <summary>
    /// BuffMechanism, float
    /// </summary>
    private Dictionary<int, float> AddDamageValueIntDict = new();
    private Dictionary<int, float> ReduceDamageValueIntDict = new();
    
    /// <summary>
    /// 获取伤害值
    /// </summary>
    /// <param name="target"></param>
    /// <param name="damageType"></param>
    /// <param name="damageSource"></param>
    /// <param name="damageWelly"></param>
    /// <param name="paramModel"></param>
    /// <returns>折前伤害，打的血量，盾，甲</returns>
    public (float, float, float, float) GetSkillDamageValue(BattleUnit target, DamageType damageType, BattleSource damageSource, float damageWelly, DamageParamModel paramModel = null)
    {
        var skillType = GetSkillType();
        var skillBase = GetSkill();
        if (skillBase == null)
        {
            return (0, 0, 0, 0);
        }
        
        //技能伤害百分比  buff伤害百分比增伤
        var damagePct = BattleMomentManager.AttackDamageAddPct(paramModel);
        var armorPiercing = skillBase.GetSkillArmorPiercing;
        AddDamageValueIntDict.Clear();
        ReduceDamageValueIntDict.Clear();
        
        if (skillType == SkillType.PowerKilling)
        {
            var getPropertySourceModel = PM.GetClass<GetPropertySourceModel>();
            getPropertySourceModel.SourceType = GetPropertySourceType.ReceiveSkillDamage;
            getPropertySourceModel.TypeID = skillBase.SkillGuid;
            getPropertySourceModel.AttackerID = EntityID;
            getPropertySourceModel.HitID = target.EntityID;
            var power = GetProperty(BattlePropertyType.Power, getPropertySourceModel);
            var damageReducePct = target.BattleMomentManager.BeDamageReducePct(EntityID, damageType);
            var defendValue = target.GetProperty(BattlePropertyType.Defend, getPropertySourceModel);
            //折前伤害的整数变量
            BattleMomentManager.AddDamageValueInt(AddDamageValueIntDict, paramModel);
            var addDamageValueInt = AddDamageValueIntDict.Values.Sum();
            var truthDamage = Math.Max(0, power * damageWelly * (1 + damagePct) + addDamageValueInt);
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
            var armorBuff = GetBuff(GameConst.Battle.ArmorBuffID);
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
            
            target.BattleMomentManager.ReduceDamageValueInt(ReduceDamageValueIntDict, paramModel);
            var reduceDamageValueInt = ReduceDamageValueIntDict.Values.Sum();
            var damageValue = Math.Max(0, truthDamage * (1 - damageReducePct) - defendValue * (1 - armorPiercing) - reduceShieldValue - reduceArmorValue - reduceDamageValueInt);
            
            PM.RecycleClass(getPropertySourceModel);
            return (truthDamage, damageValue, reduceShieldValue, reduceArmorValue);
        } 
        
        if (skillType == SkillType.ArtKilling)
        {
            var getPropertySourceModel = PM.GetClass<GetPropertySourceModel>();
            getPropertySourceModel.SourceType = GetPropertySourceType.ReceiveSkillDamage;
            getPropertySourceModel.AttackerID = EntityID;
            getPropertySourceModel.HitID = target.EntityID;
            getPropertySourceModel.TypeID = skillBase.SkillGuid;
            var tech = GetProperty(BattlePropertyType.Tech, getPropertySourceModel);
            var damageReducePct = target.BattleMomentManager.BeDamageReducePct(EntityID, damageType);
            var breakValue = target.GetProperty(BattlePropertyType.Break, getPropertySourceModel);
            //折前伤害的整数变量
            BattleMomentManager.AddDamageValueInt(AddDamageValueIntDict, paramModel);
            var addDamageValueInt = AddDamageValueIntDict.Values.Sum();
            var truthDamage = Math.Max(0, tech * damageWelly * (1 + damagePct) + addDamageValueInt);
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
            var armorBuff = GetBuff(GameConst.Battle.ArmorBuffID);
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
             
            target.BattleMomentManager.ReduceDamageValueInt(ReduceDamageValueIntDict, paramModel);
            var reduceDamageValueInt = ReduceDamageValueIntDict.Values.Sum();
            var damageValue = Math.Max(0, truthDamage * (1 - damageReducePct) - breakValue * (1 - armorPiercing) - reduceShieldValue - reduceArmorValue - reduceDamageValueInt);
            
            PM.RecycleClass(getPropertySourceModel);
            return (truthDamage, damageValue, reduceShieldValue, reduceArmorValue);
        }

        return (1, 1, 0, 0);
    }

    /// <summary>
    /// 获取伤害值
    /// </summary>
    /// <param name="truthDamage"></param>
    /// <param name="target"></param>
    /// <param name="damageType"></param>
    /// <param name="skillType"></param>
    /// <param name="damageSource"></param>
    /// <param name="paramModel"></param>
    /// <returns>折前伤害，打的血量，盾，甲</returns>
    public (float, float, float, float) GetSkillDamageValue(float truthDamage, BattleUnit target, DamageType damageType, SkillType skillType, BattleSource damageSource, DamageParamModel paramModel = null)
    {
        AddDamageValueIntDict.Clear();
        ReduceDamageValueIntDict.Clear();
        
        if (skillType == SkillType.PowerKilling)
        {
            var damageReducePct = target.BattleMomentManager.BeDamageReducePct(EntityID, damageType);
            var defendValue = target.GetProperty(BattlePropertyType.Defend);
            //折前伤害的整数变量
            BattleMomentManager.AddDamageValueInt(AddDamageValueIntDict, paramModel);
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
            var armorBuff = GetBuff(GameConst.Battle.ArmorBuffID);
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
            
            target.BattleMomentManager.ReduceDamageValueInt(ReduceDamageValueIntDict, paramModel);
            var reduceDamageValueInt = ReduceDamageValueIntDict.Values.Sum();
            var damageValue = Math.Max(0, truthDamage * (1 - damageReducePct) - defendValue - reduceShieldValue - reduceArmorValue - reduceDamageValueInt);
            
            return (truthDamage, damageValue, reduceShieldValue, reduceArmorValue);
        } 
        
        if (skillType == SkillType.ArtKilling)
        {
            var damageReducePct = target.BattleMomentManager.BeDamageReducePct(EntityID, damageType);
            var breakValue = target.GetProperty(BattlePropertyType.Break);
            //折前伤害的整数变量
            BattleMomentManager.AddDamageValueInt(AddDamageValueIntDict, paramModel);
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
            var armorBuff = GetBuff(GameConst.Battle.ArmorBuffID);
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
             
            target.BattleMomentManager.ReduceDamageValueInt(ReduceDamageValueIntDict, paramModel);
            var reduceDamageValueInt = ReduceDamageValueIntDict.Values.Sum();
            var damageValue = Math.Max(0, truthDamage * (1 - damageReducePct) - breakValue - reduceShieldValue - reduceArmorValue - reduceDamageValueInt);
     
            return (truthDamage, damageValue, reduceShieldValue, reduceArmorValue);
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
        var buff = BattleMomentManager.Buffs.TryGetValue(buffID);
        if (buff != null)
        {
            return buff.LayerCount;
        }

        return 0;
    }

    public List<BattleHeartMethodBase> GetHeartMethodList() => BattleMomentManager.HeartMethods;
    public List<BattleTreasureBase> GetTreasureList() => BattleMomentManager.Treasures;
    
    public List<BattleBuffBase> GetBuffList()
    {
        return BattleMomentManager.Buffs.GetListValue();
    }

    public BattleBuffBase GetBuff(int buffID)
    {
        return BattleMomentManager.Buffs.TryGetValue(buffID);
    }

    public bool HasBuff(int buffID)
    {
        return BattleMomentManager.Buffs.GetListKey().Contains(buffID);
    }

    public bool HasBuffType(BuffType buffType)
    {
        return BattleMomentManager.Buffs.GetListValue().Any(buff => buff.BuffType == buffType);
    }

    public BattleBuffBase AddBuff(int buffID, BattleUnit spellCaster, int addCount, List<float> paramList = null)
    {
        var buff = BattleMomentManager.Buffs.TryGetValue(buffID);
        if (buff == null)
        {
            buff = (BattleBuffBase)PM.GetClass(BattleTypeManager.GetBuffType(buffID));
            buff.AddToUnit(buffID, this, spellCaster, addCount, paramList);
            BattleMomentManager.Buffs.Add(buffID, buff);
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
                var buffConfig = ConfigManager.GetBattleBuffConfig(buffID);
                var limit = buffConfig.Limit;
                addCount = Math.Min(addCount, limit);
                var newBuff = (BattleBuffBase)PM.GetClass(BattleTypeManager.GetBuffType(buffID));
                newBuff.AddToUnit(buffID, this, spellCaster, addCount, paramList);
                BattleMomentManager.Buffs.Add(buffID, buff);
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

    public BattleHeartMethodBase AddHeartMethod(int heartMethodID)
    {
        var heartMethod = (BattleHeartMethodBase)PM.GetClass(BattleTypeManager.GetHeartMethodType(heartMethodID));
        heartMethod.Init(heartMethodID, this);
        BattleMomentManager.HeartMethods.Add(heartMethod);
        return heartMethod;
    }
    
    private BattleTreasureBase AddTreasure(int treasureID)
    {
        var treasure = (BattleTreasureBase)PM.GetClass(BattleTypeManager.GetTreasureType(treasureID));
        treasure.Init(treasureID, this);
        BattleMomentManager.Treasures.Add(treasure);
        return treasure;
    }

    public void ReduceBuffLayerCount(int buffID, int reduceCount)
    {
        var buff = BattleMomentManager.Buffs.TryGetValue(buffID);
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
        var buff = BattleMomentManager.Buffs.TryGetValue(buffID);
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
        var buff = BattleMomentManager.Buffs.TryGetValue(buffID);
        if (buff != null)
        {
            BattleMomentManager.Buffs.Remove(buffID);
            PM.RecycleClass(buff);
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
        PM.RecycleClass(Property);
        PM.RecycleClass(TakeSkillDataManager);
        PM.RecycleClass(PreUseSkillDataManager);
        PM.RecycleClass(UseSkillDataManager);
        PM.RecycleClass(BattleMomentManager);
        while (SkillSequence.Any())
        {
            var skill = SkillSequence.Dequeue();
            PM.RecycleClass(skill);
        }
        Variety.Clear();
        RoundBeDirectDamagedOpponentList.Clear();
        RoundBeDirectKillAttackOpponentList.Clear();
        InBreak = false;
        StatusPersists = 0;
        ActionTimes = 0;
        RoundBeDirectDamageTimes = 0;
        RoundAlreadyActionTimes = 0;
        SpeedCounting = 0;
        ActionWheel = 0;
        ActionWheelOut = 0;
        BeCounter = false;
        IgnoreBeCounterByKeyTypeList.Clear();
        AccumulateDamageState = false;
        AccumulateDamageValue = 0;
        RoundBeDirectDamagedOpponentList.Clear();
        RoundBeDirectKillAttackOpponentList.Clear();
        StatusPersists = 0;
        GainStatusPersists = 0;
        NotRecoverGangQiNatural = 0;
        NotRecoverXuanQiNatural = 0;
        KillUnitList.Clear();
        foreach (var model in PropDic.GetListValue())
        {
            PM.RecycleClass(model);
        }
        MaxPotionCount = 0;
        PotionIDList.Clear();
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
            PM.RecycleClass(propModel);
            return reduceCount;
        }
    }

    public void AddProp(int itemID, int count)
    {
        var propModel = PropDic.TryGetValue(itemID);
        if (propModel == null)
        {
            propModel = PM.GetClass<BattleProp>();
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
                var model = PM.GetClass<BattleRepeatUseSkill>();
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
            PM.RecycleClass(RepeatUseSkillData);
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

    #region 药水

    public int MaxPotionCount { get; set; }
    private List<int> PotionIDList = new();
    public void CheckPotion(int buffID)
    {
        if (PotionIDList.Contains(buffID))
        {
            ClearBuff(buffID);
        }
        else if (PotionIDList.Count == MaxPotionCount)
        {
            var oldID = PotionIDList[0];
            ClearBuff(oldID);
        }
    }

    public void AddPotion(int buffID)
    {
        if (!PotionIDList.Contains(buffID))
        {
            PotionIDList.Add(buffID);
        }
    }

    public void TryRemovePotion(int buffID)
    {
        if (PotionIDList.Contains(buffID))
        {
            PotionIDList.Remove(buffID);
        }
    }

    #endregion
}
