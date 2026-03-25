using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuffBase : BattleBuffMoment, IModel, IRecycle, IGetBattlePropertyChanged
{
    #region 事件
    
    private readonly List<IDisposable> _registerDisposables = new();
    //MessageManager
    protected IDisposable Register<T>(Action<T> callback) where T : MessageModel
    {
        IDisposable disposable = this.MessageManager.Register<T>(callback);
        this._registerDisposables.Add(disposable);
        return disposable;
    }
    protected void DispatchMsg<T>(T msg) where T : MessageModel => MessageManager.DispatchMsg(msg);

    #endregion
    
    public bool Valid { get; set; }
    [Inject] protected BattleUtil BattleUtil { get; set;  }
    [Inject] protected IMessageManager MessageManager { get; set;  }
    [Inject] protected ConfigManager ConfigManager { get; set; }
    [Inject] private BattleMomentConditionManager BattleMomentConditionManager { get; set; }
    [Inject] private BattleMomentManager BattleMomentManager { get; set; }
    [Inject] protected BattleManager BattleManager { get; set; }
    [Inject] protected BattleBuffManager BattleBuffManager { get; set; }
    [Inject] protected ILogManager LM { get; set; }
    public int BuffID { get; private set; }
    public BuffType BuffType { get; private set; }
    public BattleBuffConfig Config { get; private set; }
    public BattleUnit SpellCaster { get; private set; }
    public BattleUnit Subject { get; private set; }
    public BattleUnit EffectTarget { get; private set; }
    public int LayerCount { get; private set; }
    private int LastGetSumCount { get; set; } //最后一次获得buff是的总数
    public List<float> ParamList { get; } = new();
    private int Limit { get; set; }
    private int BeforeLastActionGetLayerCount { get; set; }//最后一次行动前获取的全部
    
    public virtual void AddToUnit(int buffID, BattleUnit subject, BattleUnit spellCaster, int addCount, List<float> paramList = null)
    {
        BuffID = buffID;
        Config = ConfigManager.GetBattleBuffConfig(BuffID);
        BuffType = (BuffType)Config.BuffType;
        Subject = subject;
        SpellCaster = spellCaster;
        ParamList.Clear();
        if (paramList != null)
        {
            ParamList.AddRange(paramList);
        }
        Limit = Config.Limit;
        Start();
        InitMoment(this);
        AddLayerCount(addCount);
        
        LM.D($"{subject.EntityID}得到buffID : {buffID}, 施法者 : {SpellCaster?.EntityID ?? 0}");
    }
    
    private void Start()
    {
        Valid = true;
        OnBuffStart();
        if (Config.OverlayType == (int)BuffOverlayType.Dispose)
        {
            ClearLayerCount();
        }
    }
    
    public virtual void AddLayerCount(int layerCount)
    {
        if (Limit == -1)
        {
            LayerCount += layerCount;
        }
        else
        {
            layerCount = Math.Min(Limit - LayerCount, layerCount);
            LayerCount += layerCount;
        }
        Subject.BattleChangeModelManager.BuffLayerCountChanged(BuffID, layerCount);
    }
    public bool IsMaxLayer() => LayerCount == Config.Limit;
    
    protected virtual void OnBuffStart()
    {
        
    }

    public bool CheckSkillCanUse(int skillGuid, BattleUnit target)
    {
        //如果是异常且 不能生效异常buff返回true
        if (BuffType == BuffType.Abnormal && Subject.HasBuffMechanism(BuffMechanism.NotEffectAbnormalBuff))
        {
            return true;
        }

        return OnCheckSkillCanUse(skillGuid, target);

        /*if (Config.DoDesitionMoment.Count <= 0)
            return true;

        return Config.CheckSkillDoDesitionRelation switch
        {
            1 => Config.DoDesitionMoment.All(conditionID =>
                BattleMomentConditionManager.GetCondition(conditionID, Subject, target, skillGuid, null)),
            2 => Config.DoDesitionMoment.Any(conditionID =>
                BattleMomentConditionManager.GetCondition(conditionID, Subject, target, skillGuid, null)),
            _ => false
        };*/
    }

    protected virtual bool OnCheckSkillCanUse(int skillGuid, BattleUnit target) => true;

    /// <summary>
    /// buff层数减少机制
    /// </summary>
    /// <param name="reduceType"></param>
    /// <param name="paramModel"></param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
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
            case BuffReduceType.BeforeLastActionGetAll:
                if (BeforeLastActionGetLayerCount > 0)
                {
                    reduceCount = BeforeLastActionGetLayerCount;
                    BeforeLastActionGetLayerCount = 0;
                }
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(reduceType), reduceType, null);
        }
        
        ReduceLayerCount(reduceCount);
    }
    
    
    public virtual void ReduceLayerCount(int layerCount)
    {
        if (IgnoreReduceLayer > 0)
        {
            return;
        }
        
        layerCount = Math.Min(layerCount, LayerCount - GetNotLowerLayerCount());
        layerCount = Math.Max(layerCount, 0);
        LayerCount -= layerCount;
        Subject.BattleChangeModelManager.BuffLayerCountChanged(BuffID, -layerCount);
        if (LayerCount <= 0)
        {
            Valid = false;
            OnBuffRemove();
            Subject.RemoveBuff(BuffID);
        }
    }

    protected virtual void OnBuffRemove()
    {
        
    }
    
    public virtual void ClearLayerCount()
    {
        ReduceLayerCount(LayerCount);
    }

    public void Recycle()
    {
        foreach (IDisposable registerDisposable in this._registerDisposables)
            registerDisposable.Dispose();
        this._registerDisposables.Clear();

        IgnoreReduceLayer = 0;
        BuffID = 0;
        BuffType = BuffType.None;
        Config = null;
        SpellCaster = null;
        Subject = null;
        EffectTarget = null;
        LayerCount = 0;
        ParamList.Clear();
        Limit = 0;
        LastGetSumCount = 0;
        BeforeLastActionGetLayerCount = 0;
        OnRecycle();
    }
    
    protected virtual void OnRecycle() {}

    public override void AfterAction(MomentParamModel paramModel)
    {
        BeforeLastActionGetLayerCount = LayerCount;
        base.AfterAction(paramModel);
    }

    /// <summary>
    /// buff不会减少层数
    /// </summary>
    private int IgnoreReduceLayer;
    public void SetIgnoreReduceLayer(int isIgnore) => IgnoreReduceLayer += isIgnore;

    /// <summary>
    /// buff不会低于几层
    /// </summary>
    private List<int> BuffNotLowerLayerCount = new();

    private int GetNotLowerLayerCount()
    {
        if (BuffNotLowerLayerCount.Count <= 0)
        {
            return 0;
        }

        return BuffNotLowerLayerCount.Max();
    }
    public void AddBuffNotLowerLayerCount(bool isAdd, int layerCount)
    {
        if (isAdd)
        {
            BuffNotLowerLayerCount.Add(layerCount);
        }
        else if (BuffNotLowerLayerCount.Contains(layerCount))
        {
            BuffNotLowerLayerCount.Remove(layerCount);
        }

        var min = GetNotLowerLayerCount();
        if (layerCount < min)
        {
            var delta = min - layerCount;
            AddLayerCount(delta);
        }
    }
    
    /// <summary>
    /// 触发扳机n次
    /// </summary>
    /// <param name="count"></param>
    /// <param name="paramModel"></param>
    public virtual void TriggerBuffMomentByCount(int count, MomentParamModel paramModel)
    {
        if (CanTriggerBuffEffect())
        {
            OnTriggerBuffMomentByCount(count, paramModel);
        }
    }

    protected virtual void OnTriggerBuffMomentByCount(int count, MomentParamModel paramModel) {}

    /// <summary>
    /// 触发效果n次
    /// </summary>
    /// <param name="count"></param>
    /// <param name="paramModel"></param>
    public void TriggerBuffMomentByCountIgnoreLayerCount(int count, MomentParamModel paramModel)
    {
        if (CanTriggerBuffEffect())
        {
            OnTriggerBuffMomentByCountIgnoreLayerCount(count, paramModel);
        }
    }
    
    protected virtual void OnTriggerBuffMomentByCountIgnoreLayerCount(int count, MomentParamModel paramModel) {}

    public virtual (float, float) ChangeResourceCost(float gangQiCost, float xuanQiCost)
    {
        return (gangQiCost, xuanQiCost);
    }

    public bool CheckReCalculateDamage(MomentParamModel model)
    {
        return false;
    }

    public void BeforeReduceHp(float reduceHp)
    {
        
    }

    public void KeyReplace(List<int> result, BattleKeyType keyType)
    {
        
    }

    public void ConvertChangeKey(ref BattleKeyType keyType, int count)
    {
        
    }

    public void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        
    }

    public virtual void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue, BattleSource source = BattleSource.None)
    {
        
    }

    public virtual void EndAction()
    {
        
    }

    public void RemoveBeforeNextAction()
    {
        
    }

    public virtual void BuffLayerCountChanged(int buffID, int layerCount)
    {
        
    }

    public void SetTarget(BattleUnit target) => EffectTarget = target;
    
    public float GetSkillDamageRate(MomentParamModel paramModel)
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnAddSkillDamageRate(paramModel);
    }

    protected virtual float OnAddSkillDamageRate(MomentParamModel paramModel) => 0;
    
    /// <summary>
    /// buff参与减少伤害的量
    /// </summary> dict => BuffMechanism, float  机制 改变伤害值
    /// <returns></returns>
    public virtual void AddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel) {}

    public virtual void ReduceDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel) {}

    public void AfterUnitInit()
    {
        
    }

    public void TrySetChangeActionWheel(ref int changeActionWheel)
    {
        
    }

    public void BeCounter()
    {
        
    }

    public void ReCheckClashState(ref bool state, float subjectDamageRate, float targetDamageRate)
    {
        
    }

    public virtual bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType = BattleMomentType.None)
    {
        return true;
    }

    public bool CanIgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        return false;
    }

    public bool CanBeCounter(MomentParamModel paramModel)
    {
        return true;
    }

    public float GetDamageReducePct(int attackID, DamageType damageType)
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetDamageReducePct(attackID, damageType);
    }

    public void BeforeAttack(MomentParamModel model)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
    }

    public void BeDamage(MomentParamModel model)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
    }

    public void TryStoreBattleKey(BattleKeyType keyType, ref int count)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
    }

    protected virtual float OnGetDamageReducePct(int attackID, DamageType damageType) => 0;
    
    public void KeyAdd(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
        
        OnKeyAdd(keyType, changeKeyData, reason, changeType);
    }

    protected virtual void OnKeyAdd(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType) { }
    
    public void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
        
        OnKeyReduce(keyType, changeKeyData, reason, changeType);
    }

    public void AfterChangeKey(List<BattleKey> changeKeyData, bool isAdd, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        
    }

    protected virtual void OnKeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType) { }

    public void ReduceHp(float reduceHp, DamageType damageType, int attackID)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }

        OnBeAttack(reduceHp, damageType, attackID);
    }

    protected virtual void OnBeAttack(float reduceHp, DamageType damageType, int attackID)
    {
        
    }
    
    public float GetReplaceSkillGangQiCost()
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetReplaceSkillGangQiCost();
    }
    
    protected virtual float OnGetReplaceSkillGangQiCost() => 0;

    public void EffectReplaceSkillGangQiCost(ref float gangQiDelta)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
        
        OnEffectReplaceSkillGangQiCost(ref gangQiDelta);
    }

    protected virtual void OnEffectReplaceSkillGangQiCost(ref float gangQiDelta) {}

    public float GetReplaceSkillXuanQiCost()
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetReplaceSkillXuanQiCost();
    }
    private float OnGetReplaceSkillXuanQiCost() => 0;
    
    public void EffectReplaceSkillXuanQiCost(ref float xuanQiDelta)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
        
        OnEffectReplaceSkillXuanQiCost(ref xuanQiDelta);
    }

    public void OnKillUnit(int beKillID)
    {
        
    }

    protected virtual void OnEffectReplaceSkillXuanQiCost(ref float xuanQiDelta) {}

    #region Buff加的属性放在这里
    public float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetProperty(propertyType, model);
    }
    
    protected virtual float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null) => 0;
    
    public void AfterGetProperty(BattlePropertyType propertyType, ref float value, GetPropertySourceModel model = null)
    {
        
    }
    
    #endregion

    #region 息改变量

    /// <summary>
    /// 加速或缓速
    /// </summary>
    /// <returns></returns>
    public int GetChangeActionWheel()
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetChangeActionWheel();
    }
    protected virtual int OnGetChangeActionWheel() => 0;

    #endregion

    #region 关于威力改变

    /// <summary>
    /// 获取威力改变
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <returns></returns>
    public float GetSkillWelly(int skillGuid)
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetAddWellyRate(skillGuid);
    }
    protected virtual float OnGetAddWellyRate(int skillGuid) => 0;
    
    /// <summary>
    /// 获取威力改变效果
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <returns></returns>
    public float GetSkillWellyEffect(int skillGuid)
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetAddWellyEffect(skillGuid);
    }
    protected virtual float OnGetAddWellyEffect(int skillGuid) => 0;
    
    /// <summary>
    /// 尝试设置威力基数
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public void TrySetBaseWelly(int skillGuid, ref float value)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
        
        OnTrySetBaseWellyRate(skillGuid, ref value);
    }
    protected virtual void OnTrySetBaseWellyRate(int skillGuid, ref float value) { }
    
    /// <summary>
    /// 尝试设置威力增长
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public void TrySetAddWelly(int skillGuid, ref float value)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
        
        OnTrySetAddWellyRate(skillGuid, ref value);
    }
    protected virtual void OnTrySetAddWellyRate(int skillGuid, ref float value) { }

    #endregion
    
    
    /// <summary>
    /// hp改变时
    /// </summary>
    public void HpChanged()
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
        
        OnHpChanged();
    }
    protected virtual void OnHpChanged() {}

    public int GetKeyMaxEx()
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetKeyPropertyMax();
    }
    protected virtual int OnGetKeyPropertyMax() => 0;

    public void SkillEnd(BattleSkillBase skill)
    {
        OnSkillEnd(skill);
    }

    protected virtual void OnSkillEnd(BattleSkillBase skill) {}
}
