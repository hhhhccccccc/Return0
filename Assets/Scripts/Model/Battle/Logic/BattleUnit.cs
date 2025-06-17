using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using Zenject;

public class BattleUnit : IModel
{
    #region Inject注入

    [Inject] private IPoolManager PoolManager;
    
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    
    [Inject] private IConfigManager ConfigManager;
    
    [Inject] private BattleManager BattleManager;
    
    [Inject] private BattleLogicStateManager BattleLogicStateManager;

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
    private List<BattleBuffBase> Buffs = new List<BattleBuffBase>();

    /// <summary>
    /// 携带的心法
    /// </summary>
    private List<BattleHeartMethodBase> HeartMethods = new List<BattleHeartMethodBase>();
    
    /// <summary>
    /// 携带的宝器
    /// </summary>
    private List<BattleTreasureBase> Treasures = new List<BattleTreasureBase>();
    
    private BattleSkillBase SkillBase;
    
    public void SetSkill(int skillID, BattleUnit target)
    {
        SkillBase = PoolManager.GetClass<BattleSkillBase>();
        SkillBase.Init(skillID, this, target);
    }

    public void ClearSkill()
    {
        PoolManager.RecycleClass(SkillBase);
        SkillBase = null;
    }

    #endregion
    
    
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
        
        //键有关
        IgnoreBeCounterByDamage = 0;
        IgnoreBeCounterByCount = 0;
        IgnoreBeCounterByKeyTypeList.Clear();
        
        //伤害
        AccumulateDamageState = false;
        AccumulateDamageValue = 0;
        TempSkillDamageAddValue = 0;
    }
    
    /// <summary>
    /// 行动结束
    /// </summary>
    public void ActionEnd()
    {
        ReduceActionTimes();
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
            
        }
        AccumulateDamageValue = 0;
        TempSkillDamageAddValue = 0;
    }

    public bool IsAlive()
    {
        return GetProperty("Hp") > 0;
    }
    
    private List<IBattleMoment> TempBattleMoment = new();
     
    public List<IBattleMoment> GetBattleMoment()
    {
        TempBattleMoment.Clear();
        TempBattleMoment.AddRange(Treasures);
        TempBattleMoment.AddRange(HeartMethods);
        TempBattleMoment.AddRange(Buffs);
        if (SkillBase != null)
        {
            TempBattleMoment.Add(SkillBase);
        }
        return TempBattleMoment;
    }

    #region 状态判断

    public bool IsBeCounter()
    {
        return Buffs.Any(buff => buff.Cfg.Id == 1);
    }

    #endregion

    #region 属性
    public bool ChangeProperty(string propName, int propValue)
    {
        return Property.ChangeProperty(propName, propValue);
    }

    public bool SetProperty(string propName, int propValue)
    {
        return Property.SetProperty(propName, propValue);
    }

    public int GetProperty(string propName)
    {
        return Property.GetProperty(propName);
    }

    public bool AddKey(BattleKey key, int count = 1)
    {
        return Property.AddKey(key, count);
    }
    
    public bool CostKey(BattleKey key, int count = 1)
    {
        return Property.CostKey(key, count);
    }

    public int GetKeyCount => Property.KeyCount;

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
    /// <summary>
    /// 不会被破招的键的列表
    /// </summary>
    private List<BattleKey> IgnoreBeCounterByKeyTypeList = new();
    public void AddIgnoreBeCounterKey(BattleKey key) => IgnoreBeCounterByKeyTypeList.Add(key);
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
        var config = ConfigManager.GetBattleSkill(skillID);
        var needKey = config.NeedKey;
        //破招失败
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
        var fastMax = BattleLogicStateManager.ActionWheel;
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

    private int AccumulateDamageValue;
    
    private bool AccumulateDamageState;
    public bool SetAccumulateDamage => AccumulateDamageState = true;

    public virtual void BeDamage(int damage)
    {
        if (AccumulateDamageState)
        {
            AccumulateDamageValue += damage;
        }
        else
        {
            
        }
    }

    private int TempSkillDamageAddValue;

    public void AddTempSkillDamageValue(int damageAddValue) => TempSkillDamageAddValue += damageAddValue;
    //private float TempSkillDamageValuePct;
    
    public int GetSkillDamage()
    {
        if (SkillBase == null)
            return 0;

        return SkillBase.GetSkillDamageValue() + TempSkillDamageAddValue;
    }

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
    
    #endregion
}
