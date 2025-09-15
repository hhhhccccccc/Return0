using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;


public class BattleUnit : IModel
{
    #region Inject注入
    [Inject] private IPoolManager PoolManager { get; set; }
    
    [Inject] private ILogManager LogManager { get; set; }
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] private ConfigManager ConfigManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    [Inject] private BattleLogicStateManager BattleLogicStateManager { get; set; }
    [Inject] private BattleUtil BattleUtil { get; set; }
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    
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
        TryAddSkillPreUseData(skillID);
        var skillBase = PoolManager.GetClass<BattleSkillBase>();
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
                (skillBase.GetRemoveMomentList.Contains((int)type) && type == SkillRemoveMomentType.BeforeNextAction && skillBase.CheckTriggerMoment(BattleMomentType.AfterAction)))
            {
                skillBase.SkillEnd();
                TryAddSkillPreUseCount(skillBase.SkillID);
                PoolManager.RecycleClass(skillBase);
            }
        }
    }

    private static Dictionary<string, Type> SkillPreUseDataNameToType = new();

    private Dictionary<int, BattleSkillUseDataBase> SkillPreUseDataDict = new();

    private void TryAddSkillPreUseData(int skillID)
    {
        if (!SkillPreUseDataDict.TryGetValue(skillID, out var data))
        {
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            var useDataScript = config.SkillPreUseDataScript;
            if (string.IsNullOrEmpty(useDataScript))
            {
                data = PoolManager.GetClass<BattleSkillUseDataBase>();
                data.SkillID = skillID;
                data.UseCount = 0;
            }
            else
            {
                if (!SkillPreUseDataNameToType.TryGetValue(useDataScript, out var type))
                {
                    type = Type.GetType(useDataScript);
                    SkillPreUseDataNameToType.Add(useDataScript, type);
                }

                data = (BattleSkillUseDataBase)PoolManager.GetClass(type);
                data.SkillID = skillID;
                data.UseCount = 0;
            }
           
            SkillPreUseDataDict.Add(skillID, data);
        }
    }

    private void TryAddSkillPreUseCount(int skillID, int count = 1)
    {
        if (SkillPreUseDataDict.TryGetValue(skillID, out var data))
        {
            data.UseCount += count;
        }
    }

    public BattleSkillUseDataBase GetSkillPreUseData(int skillID)
    {
        if (SkillPreUseDataDict.TryGetValue(skillID, out var data))
        {
            return data;
        }

        return null;
    }

    /// <summary>
    /// 算上减少消耗的百分比或者固定值 用于面板显示
    /// </summary>
    /// <param name="skillID"></param>
    /// <returns></returns>
    public float GetSkillPreUseGangQiCost(int skillID)
    {
        var preData = GetSkillPreUseData(skillID);
        if (preData == null)
        {
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return GetGangQiCost(config.GangQiCost);
        }

        return GetGangQiCost(preData.GetGangQiCost());
    }
    
    public float GetSkillPreUseXuanQiCost(int skillID)
    {
        var preData = GetSkillPreUseData(skillID);
        if (preData == null)
        {
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            return GetXuanQiCost(config.XuanQiCost) ;
        }

        return GetXuanQiCost(preData.GetXuanQiCost());
    }

    #endregion
    
    public bool IsSelf { get; set; }
    public float ActionRadius { get; set; }
    public float ClashRadius { get; set; }
    public int Bgm { get; set; }
    public virtual void Init(BattleField bf, HeroData heroData)
    {
        Bf = bf;
        IsSelf = bf.Uid == 1;
        HeroData = heroData;
        SlotIndex = heroData.SlotIndex;
        BattleManager.ResetUnitToDict(this);
        Property = PoolManager.GetClass<BattleProperty>();
        Property.Init(heroData);
        WearSkillList = HeroData.WearSkillList;
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
        Bgm = HeroData.GetFightProperty_Bgm();
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
        //伤害
        AccumulateDamageState = false;
        if (AccumulateDamageValue > 0)
        {
            if (ReduceHp(AccumulateDamageValue, DamageType.InDirect))
            {
                
            }
        }
        AccumulateDamageValue = 0;
        
        TempSkillDamageAddValue = 0;

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
        
        TempSkillDamageAddValue = 0;
        SetBeCounter(false);
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

    public bool TryCalculateNextActionWheel()
    {
        if (ActionTimes > 0 && !BattleLogicBehaviourManager.BattleBehaviourRes.ContainsKey(EntityID))
        {
            ActionWheel = BattleLogicStateManager.ActionWheel + 1;
            return true;
        }

        return false;
    }
    
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
        
        var config = ConfigManager.GetBattleSkillConfig(skillID);
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
    public virtual void BeDamage(ref DamageParamModel model)
    {
        var allDamage = model.HitDamageValue;
        if (model.HitDamageType == DamageType.Direct)
        {
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
        var skillBase = GetSkill();
        if (skillBase == null)
            return 0;

        return skillBase.GetSkillDamageRate;
    }

    public float GetSkillDamageRateSum()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return 0;

        return skillBase.GetSkillDamageRate + TempSkillDamageAddValue;
    }
    
    public float GetSkillDamageRateFight()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return 0;
        
        return skillBase.GetSkillDamageRate + TempSkillDamageAddValue;
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

    public void SetSkillType(SkillType type)
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return;
        
        skillBase.SetSkillType(type);
    }

    public DamageType GetSkillDamageType()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return DamageType.None;

        return skillBase.GetDamageType;
    }
    
    public void SetSkillDamageType(DamageType type)
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return;
        
        skillBase.SetDamageType(type);
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
    
    public void SetBeDamageInSkillAction()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return;
        
        skillBase.SetBeDamageInSkillAction();
    }

    public bool GetBeDamageInSkillAction()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return false;
        return skillBase.GetBeDamageInSkillAction();
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

    private float RecoverGangQiBySkillReduce;
    public void ChangeRecoverGangQiBySkillReduce(float value) => RecoverGangQiBySkillReduce += value;
    
    private float RecoverXuanQiBySkillReduce;
    public void ChangeRecoverXuanQiBySkillReduce(float value) => RecoverXuanQiBySkillReduce += value;

    /// <summary>
    /// 用于判断招式刚气是否足够
    /// </summary>
    /// <param name="gangQiCost"></param>
    /// <returns>返回的是正数 后面调用的是招式的消耗减少增量</returns>
    public float GetGangQiCost(float gangQiCost)
    {
        return Math.Max((gangQiCost * (1 - GetProperty(BattlePropertyType.GangQiRedPct)) -
                         GetProperty(BattlePropertyType.GangQiRedInt)) *
                        (1 - GetProperty(BattlePropertyType.AllGangQiRedPct)), 0);
    }
    
    /// <summary>
    /// 用于判断招式玄气是否足够
    /// </summary>
    /// <param name="gangQiCost"></param>
    /// <returns>返回的是正数 后面调用的是招式的消耗减少增量</returns>
    public float GetXuanQiCost(float XuanQiCost)
    {
        return Math.Max((XuanQiCost * (1 - GetProperty(BattlePropertyType.XuanQiRedPct)) -
                         GetProperty(BattlePropertyType.XuanQiRedInt)) *
                        (1 - GetProperty(BattlePropertyType.AllXuanQiRedPct)), 0);
    }
    #endregion

    #region 技能方法

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
        var costGangQi = GetGangQiCost(skillBase.GetGangQiCost());
        if (hasGangQi < costGangQi)
             return false;
        
        var hasXuanQi = GetProperty(BattlePropertyType.XuanQi);
        var costXuanQi = GetGangQiCost(skillBase.GetXuanQiCost());
        if (hasXuanQi < costXuanQi)
            return false;
        
        foreach (var (keyType, keyCount) in Util.KeyListToDictionary(skillBase.GetKeyCostList))
        {
            var hasKey = GetKey((BattleKeyType)keyType);
            if (hasKey < keyCount)
                return false;
        }
        
        return true;
    }

    public bool CheckReleaseSkillEnough(int skillID)
    {
        var config = ConfigManager.GetBattleSkillConfig(skillID);
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
    public (float, float, List<int>) CostSkillNeedResource()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
        {
            return (0, 0, new List<int>());
        }
        var gangQiCost = skillBase.GetGangQiCost();
        ChangeProperty(BattlePropertyType.GangQi, -gangQiCost, BattleSource.Skill);
        var xuanQiCost = skillBase.GetXuanQiCost();
        ChangeProperty(BattlePropertyType.XuanQi, -xuanQiCost, BattleSource.Skill);
        var keyCost = skillBase.GetKeyCostList;
        foreach (var (keyType, keyCount) in Util.KeyListToDictionary(keyCost))
        {
            ChangeKey((BattleKeyType)keyType, keyCount);
        }

        return (gangQiCost, xuanQiCost, keyCost);
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

    public BattlePropertyType GetSkillFirstKey()
    {
        var skillBase = GetSkill();
        if (skillBase == null)
            return BattlePropertyType.None;

        return skillBase.GetFirstKeyType();
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

    public BattleBuffBase AddBuff(int buffID, BattleUnit spellCaster, int addCount, List<float> paramList = null)
    {
        var buff = Buffs.TryGetValue(buffID);
        if (buff == null)
        {
            buff = (BattleBuffBase)PoolManager.GetClass(BattleBuffManager.GetBuffType(buffID));
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
        if (buffType == BuffType.None)
        {
            buffList = buffList.Where(buff => buff.BuffType == buffType).ToList();
        }
        var weightList = Util.GetSameChanceList(buffList.Count);
        return Util.GetRandomNoSame(buffList, weightList, count);
    }

    #endregion
}
