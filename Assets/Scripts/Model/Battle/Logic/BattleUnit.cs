using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using cfg;
using Codice.LogWrapper;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

public class BattleUnit : IModel
{
    #region Inject注入

    [Inject] private IPoolManager PoolManager;
    [Inject] private ILogManager LogManager;
    
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    
    [Inject] private ConfigManager ConfigManager;
    
    [Inject] private BattleManager BattleManager;
    
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    
    [Inject] private BattleUtil BattleUtil;
    
    [Inject] private BattleBuffManager BattleBuffManager;
    
    [Inject] private BattleRecordManager BattleRecordManager;

    #endregion
    
    public BattleField Bf;
    
    public int EntityID;

    public int SlotIndex;
    
    protected BattleObjType ObjType;

    private BattleProperty Property;

    #region 携带的Buff,心法,宝器,当前释放的技能

    /// <summary>
    /// 携带的buff
    /// </summary>
    private DictAndList<int, BattleBuffBase> Buffs = new DictAndList<int, BattleBuffBase>();
    
    /// <summary>
    /// 携带的心法
    /// </summary>
    private List<BattleHeartMethodBase> HeartMethods = new List<BattleHeartMethodBase>();
    
    /// <summary>
    /// 携带的宝器
    /// </summary>
    private List<BattleTreasureBase> Treasures = new List<BattleTreasureBase>();
    
    private BattleSkillBase SkillBase;
    public BattleSkillBase GetSkillBase => SkillBase;
    
    public void SetSkill(int skillID, BattleUnit target)
    {
        SkillBase = PoolManager.GetClass<BattleSkillBase>();
        SkillBase.Init(skillID, this, target);
    }

    public void ClearSkill()
    {
        SkillBase.SkillEnd();
        PoolManager.RecycleClass(SkillBase);
        SkillBase = null;
    }

    #endregion

    private float Shield;
    
    public bool IsSelf;
    public virtual void Init(BattleField bf, Character character, int slotIndex)
    {
        Bf = bf;
        IsSelf = bf.Uid == 1;
        SlotIndex = slotIndex;
        BattleManager.ResetUnitToDict(this);
        Property = new BattleProperty();
        Property.Init(character);
    }
    /// <summary>
    /// 回合开始
    /// </summary>
    public virtual void RoundStart()
    {
        //行动有关
        ActionTimes = 1;
        SpeedCounting = 0;
        ActionWheel = 0;
        ActionWheelOut = 0;
        DontBeCounter = 0;
        
        //键有关
        IgnoreBeCounterByDamage = 0;
        IgnoreBeCounterByCount = 0;
        IgnoreBeCounterByKeyTypeList.Clear();
        
        //伤害
        AccumulateDamageState = false;
        AccumulateDamageValue = 0;
        TempSkillDamageAddValue = 0;

        if (BattleLogicStateManager.Round != 1)
        {
            RecoverGangQiNatural();
            RecoverXuanQiNatural();
        }
    }
    
    /// <summary>
    /// 这一息结束
    /// </summary>
    public void OneActionWheelEnd()
    {
        BeCounter = false;
        ActionWheelOut = 0;
    }
    
    /// <summary>
    /// 回合结束
    /// </summary>
    public virtual void RoundEnd()
    {
        //行动有关
        ActionTimes = 0;
        SpeedCounting = 0;
        ActionWheel = 0;
        ActionWheelOut = 0;

        //键有关
        IgnoreBeCounterByDamage = 0;
        IgnoreBeCounterByCount = 0;
        IgnoreBeCounterByKeyTypeList.Clear();
        
        //伤害
        AccumulateDamageState = false;
        if (AccumulateDamageValue > 0)
        {
            if (ReduceHp(AccumulateDamageValue))
            {
                
            }
        }
        AccumulateDamageValue = 0;
        TempSkillDamageAddValue = 0;
        SetBeCounter(false);
    }

    public bool IsAlive()
    {
        return GetProperty(BattlePropertyType.Hp) > 0;
    }
    
    private List<IBattleMoment> TempBattleMoment = new();
     
    public List<IBattleMoment> GetBattleMoment()
    {
        TempBattleMoment.Clear();
        TempBattleMoment.AddRange(Treasures);
        TempBattleMoment.AddRange(HeartMethods);
        TempBattleMoment.AddRange(GetBuffList());
        if (SkillBase != null)
        {
            TempBattleMoment.Add(SkillBase);
        }
        return TempBattleMoment;
    }

    protected virtual void Die()
    {
        
    }

    #region 属性
    public bool ChangeProperty(BattlePropertyType propType, float propValue, BattleSource source = BattleSource.None)
    {
        if (propType == BattlePropertyType.GangQi && propValue > 0 && source == BattleSource.Skill)
        {
            propValue = Math.Max(propValue - RecoverGangQiBySkillReduce, 0);
        }
        
        if (propType == BattlePropertyType.XuanQi && propValue > 0 && source == BattleSource.Skill)
        {
            propValue = Math.Max(propValue - RecoverXuanQiBySkillReduce, 0);
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

    private List<int> TempKeyList = new();

    public List<int> GetKeyList()
    {
        TempKeyList.Clear();
        foreach (var keyType in Util.KeyList)
        {
            for (int i = 1; i <= GetKey(keyType);i++)
            {
                TempKeyList.Add(GetKey(keyType));
            }
        }

        return TempKeyList;
    }

    public int ActionTimes;
    public void ReduceActionTimes() => ActionTimes--;
    public float SpeedCounting;
    //下一行动息值
    public int ActionWheel;
    //息溢值
    public int ActionWheelOut;
    //是否被破招了
    private bool BeCounter;
    public bool GetBeCounter() => BeCounter;
    public void SetBeCounter(bool state) => BeCounter = state;

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
    private bool DontBeCounterByPowerKilling;
    public void SetDontBeCounterByPowerKilling(bool state) => DontBeCounterByPowerKilling = state;
    /// <summary>
    /// 不会被武杀式破招
    /// </summary>
    private bool DontBeCounterByArtKilling;
    public void SetDontBeCounterByArtKilling(bool state) => DontBeCounterByArtKilling = state;
    
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
    /// <summary>
    /// 免疫几次破招
    /// </summary>
    private int IgnoreBeCounterByCount;
    public void AddIgnoreBeCountByCount(int count) => IgnoreBeCounterByDamage += count;
    /// <summary>
    /// 尝试被破招
    /// </summary>
    public bool TryBeCounter(int skillID)
    {
        //破招失败
        if (DontBeCounter > 0)
        { 
            return false;
        }
        
        var config = ConfigManager.GetBattleSkill(skillID);
        var needKey = config.NeedKey;

        if (DontBeCounterByPowerKilling && BattleUtil.GetSkillTypeBySkillID(skillID) == SkillType.PowerKilling)
        {
            return false;
        }

        if (DontBeCounterByArtKilling && BattleUtil.GetSkillTypeBySkillID(skillID) == SkillType.ArtKilling)
        {
            return false;
        }
        
        if (IgnoreBeCounterByKeyTypeList.Any(hasKey => needKey.Contains((int)hasKey)))
        {
            return false;
        }

        if (IgnoreBeCounterByDamage > 0)
        {
            IgnoreBeCounterByDamage--;
            return false;
        }

        if (IgnoreBeCounterByCount > 0)
        {
            IgnoreBeCounterByCount--;
            return false;
        }

        BeCounter = true;
        return true;
    }
    
    //改变息
    public void ChangeActionWheel(int value)
    {
        var fastMax = BattleLogicStateManager.GetAfterStartActionWheel
            ? BattleLogicStateManager.ActionWheel + 1
            : BattleLogicStateManager.ActionWheel;
        
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

    /// <summary>
    /// 延迟受伤
    /// </summary>
    private bool AccumulateDamageState;
    public void SetAccumulateDamage() => AccumulateDamageState = true;
    
    private float AccumulateDamageValue;
    public virtual void BeDamage(ref DamageParamModel model)
    {
        var allDamage = model.HitDamageValue;
        foreach (var buff in GetBuffList())
        {
            model.HitShieldValue += buff.ReduceShield(ref allDamage);
        }

        model.HitHpValue = allDamage;

        if (model.HitHpValue > 0)
        {
            if (AccumulateDamageState)
            {
                AccumulateDamageValue += model.HitHpValue;
            }
            else
            {
                if (ReduceHp(model.HitHpValue))
                {
                    
                }
            }
        }
    }

    /// <summary>
    /// 扣血
    /// </summary>
    /// <param name="reduceHp"></param>
    /// <returns>是否死亡</returns>
    protected virtual bool ReduceHp(float reduceHp)
    {
        ChangeProperty(BattlePropertyType.Hp, -reduceHp);
        var isDie = GetProperty(BattlePropertyType.Hp) <= 0;
        if (isDie)
        {
            Die();
        }
        return isDie;
    }

    private float TempSkillDamageAddValue;

    #region 技能方法

    public void AddTempSkillDamageValue(float damageAddValue) => TempSkillDamageAddValue += damageAddValue;
    
    //private float TempSkillDamageValuePct;
    public float GetSkillDamageRate()
    {
        if (SkillBase == null)
            return 0;

        return SkillBase.GetSkillDamageRate;
    }

    public float GetSkillDamageRateSum()
    {
        if (SkillBase == null)
            return 0;

        return SkillBase.GetSkillDamageRate + TempSkillDamageAddValue;
    }
    
    public float GetSkillDamageRateFight()
    {
        if (SkillBase == null)
            return 0;
        
        return SkillBase.GetSkillDamageRate + TempSkillDamageAddValue;
    }

    public int GetSkillID()
    {
        if (SkillBase == null)
            return 0;

        return SkillBase.GetSkillID();
    }
    
    public string GetSkillAniName()
    {
        if (SkillBase == null)
            return string.Empty;

        return SkillBase.GetSkillAniName();
    }
    
    public SkillType GetSkillType()
    {
        if (SkillBase == null)
            return SkillType.None;

        return SkillBase.GetSKillType;
    }

    public void SetSkillType(SkillType type)
    {
        if (SkillBase == null)
            return;
        
        SkillBase.SetSkillType(type);
    }

    public DamageType GetSkillDamageType()
    {
        if (SkillBase == null)
            return DamageType.None;

        return SkillBase.GetDamageType;
    }
    
    public void SetSkillDamageType(DamageType type)
    {
        if (SkillBase == null)
            return;
        
        SkillBase.SetDamageType(type);
    }

    #endregion
   

    public void ForceSetSkill(int newSkillID)
    {
        if (SkillBase != null)
        {
            ClearSkill();
        }

        var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(EntityID);
        var target = BattleManager.GetUnit(behaviour.TargetID);
        //todo 判断目标是否合法
        behaviour.SkillID = newSkillID;
        SetSkill(newSkillID, target);
    }

    public void ForceChangeTarget(int newTargetID)
    {
        if (SkillBase == null)
            return;
        var skillID = SkillBase.SkillID;
        var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(EntityID);
        behaviour.TargetID = newTargetID;
        var newTarget = BattleManager.GetUnit(newTargetID);
        ClearSkill();
        SetSkill(skillID, newTarget);
    }

    private void RecoverGangQiNatural()
    {
        ChangeProperty(BattlePropertyType.GangQi, GetProperty(BattlePropertyType.GangQiRecNatural), BattleSource.Natural);
    }
    
    private void RecoverXuanQiNatural()
    {
        ChangeProperty(BattlePropertyType.XuanQi, GetProperty(BattlePropertyType.XuanQiRecNatural), BattleSource.Natural);
    }
    
    public void SetBeDamageInSkillAction()
    {
        if (SkillBase != null)
        {
            SkillBase.SetBeDamageInSkillAction();
        }
    }

    public bool GetBeDamageInSkillAction()
    {
        if (SkillBase == null)
            return false;

        return SkillBase.GetBeDamageInSkillAction();
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

    public bool AddKey(BattleKeyType keyType, int value) => Property.ChangeKey(keyType, value);
    
    public bool ChangeKey(BattleKeyType propType, int value) => Property.ChangeKey(propType, value);

    public int GetKeyCount() => Property.GetKeyCount();
    public void RemoveAllKey()
    {
        SetKey(BattleKeyType.KeyUp, 0);
        SetKey(BattleKeyType.KeyDown, 0);
        SetKey(BattleKeyType.KeyLeft, 0);
        SetKey(BattleKeyType.KeyRight, 0);
    }

    #endregion

   

    private float RecoverGangQiBySkillReduce;
    public float AddRecoverGangQiBySkillReduce(float value) => RecoverGangQiBySkillReduce += value;
    
    private float RecoverXuanQiBySkillReduce;
    public float AddRecoverXuanQiBySkillReduce(float value) => RecoverXuanQiBySkillReduce += value;

    /// <summary>
    /// 用于判断招式刚气是否足够
    /// </summary>
    /// <param name="gangQiCost"></param>
    /// <returns>返回的是正数 后面调用的是招式的消耗减少增量</returns>
    public float GetGangQiCost(float gangQiCost)
    {
        return (gangQiCost * (1 - GetProperty(BattlePropertyType.GangQiRedPct)) -
                      GetProperty(BattlePropertyType.GangQiRedInt)) * (1 - GetProperty(BattlePropertyType.AllGangQiRedPct));
    }
    
    /// <summary>
    /// 用于判断招式玄气是否足够
    /// </summary>
    /// <param name="gangQiCost"></param>
    /// <returns>返回的是正数 后面调用的是招式的消耗减少增量</returns>
    public float GetXuanQiCost(float XuanQiCost)
    {
        return (XuanQiCost * (1 - GetProperty(BattlePropertyType.XuanQiRedPct)) -
                GetProperty(BattlePropertyType.XuanQiRedInt)) * (1 - GetProperty(BattlePropertyType.AllXuanQiRedPct));
    }
    #endregion

    #region 技能方法

    /// <summary>
    /// 检查技能是否能释放成功
    /// </summary>
    /// <returns></returns>
    public bool CheckReleaseSkillEnough()
    {
        if (SkillBase == null)
            return false;

        var hasGangQi = GetProperty(BattlePropertyType.GangQi);
        var costGangQi = GetGangQiCost(SkillBase.GetGangQiCost());
        if (hasGangQi < costGangQi)
            return false;
        
        var hasXuanQi = GetProperty(BattlePropertyType.XuanQi);
        var costXuanQi = GetGangQiCost(SkillBase.GetXuanQiCost());
        if (hasXuanQi < costXuanQi)
            return false;
        
        foreach (var (keyType, keyCount) in Util.KeyListToDictionary(SkillBase.GetKeyCostList))
        {
            var hasKey = GetKey((BattleKeyType)keyType);
            if (hasKey < keyCount)
                return false;
        }
        
        return true;
    }

    public bool CheckReleaseSkillEnough(int skillID)
    {
        var config = ConfigManager.GetBattleSkill(skillID);
        if (config == null)
            return false;
        var hasGangQi = GetProperty(BattlePropertyType.GangQi);
        var costGangQi = GetGangQiCost(config.GangQiCost);
        if (hasGangQi < costGangQi)
            return false;
        
        var hasXuanQi = GetProperty(BattlePropertyType.XuanQi);
        var costXuanQi = GetGangQiCost(config.XuanQiCost);
        if (hasXuanQi < costXuanQi)
            return false;
        
        foreach (var (keyType, keyCount) in Util.KeyListToDictionary(config.NeedKey))
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
    public void CostSkillNeedResource()
    {
        ChangeProperty(BattlePropertyType.GangQi, -SkillBase.GetGangQiCost());
        ChangeProperty(BattlePropertyType.XuanQi, -SkillBase.GetXuanQiCost());
        foreach (var (keyType, keyCount) in Util.KeyListToDictionary(SkillBase.GetKeyCostList))
        {
            ChangeProperty((BattlePropertyType)keyType, keyCount);
        }
    }
    
    public float GetSkillKillDamageValue(BattleUnit target, DamageType damageType, BattleSource damageSource, float damageRate)
    {
        var skillType = GetSkillType();    
            
        if (skillType == SkillType.PowerKilling)
        {
            var power = GetProperty(BattlePropertyType.Power);
            var skillDamageRateSum = damageRate;
            var skillDamageRateFloor = GetProperty(BattlePropertyType.SkillDamageRateFloor);
            var damageReducePct = target.GetProperty(BattlePropertyType.DamageReducePct);
            var killDamageReduceInt = target.GetProperty(BattlePropertyType.KillingDamageReduceInt);
            var defendValue = target.GetProperty(BattlePropertyType.Defend);
            return power * skillDamageRateSum * (1 + skillDamageRateFloor) * (1 - damageReducePct) - killDamageReduceInt - defendValue;
        } 
        
        if  (skillType == SkillType.ArtKilling)
        {
            var tech = GetProperty(BattlePropertyType.Tech);
            var skillDamageRateSum = damageRate;
            var skillDamageRateFloor = GetProperty(BattlePropertyType.SkillDamageRateFloor);
            var damageReducePct = target.GetProperty(BattlePropertyType.DamageReducePct);
            var killDamageReduceInt = target.GetProperty(BattlePropertyType.KillingDamageReduceInt);
            var breakValue = target.GetProperty(BattlePropertyType.BreakInt);
            return tech * skillDamageRateSum * (1 + skillDamageRateFloor) * (1 - damageReducePct) - killDamageReduceInt - breakValue;
        }

        return 0;
    }

    public bool SkillIsKillingStyle()
    {
        if (SkillBase == null)
            return false;

        return SkillBase.SkillIsKillingStyle();
    }

    public BattlePropertyType GetSkillFirstKey()
    {
        if (SkillBase == null)
        {
            return BattlePropertyType.None;
        }

        return SkillBase.GetFirstKeyType();
    }

    public bool CheckSkillCanUse(int skillID)
    {
        return GetBuffList().All(buff => buff.CheckSkillCanUse(skillID)) && CheckReleaseSkillEnough(skillID);
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

    public void AddBuff(int buffID, BattleUnit spellCaster, int addCount, List<float> paramList = null)
    {
        var buff = Buffs.TryGetValue(buffID);
        if (buff == null)
        {
            buff = (BattleBuffBase)PoolManager.GetClass(BattleBuffManager.GetBuffType(buffID));
            buff.AddToUnit(buffID, this, spellCaster, addCount, paramList);
            Buffs.Add(buffID, buff);
        }
        else
        {
            var config = ConfigManager.GetBattleBuff(buffID);
            if (config.OverlayType == (int)BuffOverlayType.Cover)
            {
                buff.ClearLayerCount();
                var newBuff = PoolManager.GetClass<BattleBuffBase>();
                newBuff.AddToUnit(buffID, this, spellCaster, addCount, paramList);
            }
            else if (config.OverlayType == (int)BuffOverlayType.Overlap)
            {
                buff.AddLayerCount(addCount);
            }
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

    public void ClearBuff(int buffID)
    {
        var buff = Buffs.TryGetValue(buffID);
        if (buff != null)
        {
            buff.ClearLayerCount();
        }
    }

    public void RemoveBuff(int buffID)
    {
        var buff = Buffs.TryGetValue(buffID);
        if (buff != null)
        {
            Buffs.Remove(buffID);
            PoolManager.RecycleClass(buff);
        }
    }

    #endregion
}
