using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuffBase : BattleBuffMoment, IModel, IRecycle, IBattlePropertyChanged
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
    [Inject] protected IMessageManager MessageManager { get; set; }
    [Inject] private ConfigManager ConfigManager { get; set; }
    [Inject] private BattleMomentConditionManager BattleMomentConditionManager { get; set; }
    [Inject] private BattleMomentManager BattleMomentManager { get; set; }
    [Inject] protected BattleBuffManager BattleBuffManager { get; set; }
    [Inject] private ILogManager LogManager { get; set; }
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
        
        LogManager.Debug($"{subject.EntityID}得到buffID : {buffID}, 施法者 : {SpellCaster?.EntityID ?? 0}");
    }
    
    private void Start()
    {
        Valid = true;
        var subjectID = Subject.EntityID;
        var spellCasterID = SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Config.BuffStartMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, 0, BattleMomentType.BuffStart));
        }
        OnStart();
        if (Config.OverlayType == (int)BuffOverlayType.Dispose)
        {
            ClearLayerCount();
        }
    }
    
    public virtual void AddLayerCount(int layerCount)
    {
        if (Limit == -1)
        {
            TriggerBuffAddLayerMoment(layerCount);
            LayerCount += layerCount;
        }
        else
        {
            layerCount = Math.Min(Limit - LayerCount, layerCount);
            TriggerBuffAddLayerMoment(layerCount);
            LayerCount += layerCount;
        }
        OnLayerCountChanged();
    }

    protected virtual void OnLayerCountChanged() {}
    
    public bool IsMaxLayer() => LayerCount == Config.Limit;
    
    protected virtual void OnStart()
    {
        
    }

    private void TriggerBuffAddLayerMoment(int addLayerCount)
    {
        var subjectID = Subject.EntityID;
        var spellCasterID = SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Config.BuffAddLayerMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, addLayerCount, BattleMomentType.BuffAddLayer));
        }
    }

    private void TriggerBuffReduceLayerMoment(int reduceLayerCount)
    {
        var subjectID = Subject.EntityID;
        var spellCasterID = SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Config.BuffReduceMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, reduceLayerCount, BattleMomentType.BuffReduceLayer));
        }
    }

    public virtual bool CheckSkillCanUse(int skillGuid, BattleUnit target)
    {
        //如果是异常且 不能生效异常buff返回true
        if (BuffType == BuffType.Abnormal && Subject.HasBuffMechanism(BuffMechanism.NotEffectAbnormalBuff))
        {
            return true;
        }
        
        if (Config.DoDesitionMoment.Count <= 0)
            return true;

        return Config.CheckSkillDoDesitionRelation switch
        {
            1 => Config.DoDesitionMoment.All(conditionID =>
                BattleMomentConditionManager.GetCondition(conditionID, Subject, target, skillGuid, null)),
            2 => Config.DoDesitionMoment.Any(conditionID =>
                BattleMomentConditionManager.GetCondition(conditionID, Subject, target, skillGuid, null)),
            _ => false
        };
    }
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
    
    public void ReduceLayerCount(int layerCount)
    {
        if (IgnoreReduceLayer)
        {
            return;
        }
        
        layerCount = Math.Min(layerCount, LayerCount - BuffNotLowerLayerCount);
        layerCount = Math.Max(layerCount, 0);
        LayerCount -= layerCount;
        OnLayerCountChanged();
        TriggerBuffReduceLayerMoment(layerCount);
        if (LayerCount <= 0)
        {
            var subjectID = Subject.EntityID;
            var spellCasterID = SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Config.BuffRemoveMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, 0, BattleMomentType.BuffRemove));
            }
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

    public virtual void Recycle()
    {
        foreach (IDisposable registerDisposable in this._registerDisposables)
            registerDisposable.Dispose();
        this._registerDisposables.Clear();
        
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
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        BeforeLastActionGetLayerCount = LayerCount;
        base.AfterAction(paramModel);
    }

    /// <summary>
    /// buff不会减少层数
    /// </summary>
    private bool IgnoreReduceLayer;
    public void SetIgnoreReduceLayer(bool isIgnore) => IgnoreReduceLayer = isIgnore;

    /// <summary>
    /// buff不会低于几层
    /// </summary>
    private int BuffNotLowerLayerCount;
    public void SetBuffNotLowerLayerCount(int layerCount) => BuffNotLowerLayerCount += layerCount;
    
    /// <summary>
    /// 触发扳机n次
    /// </summary>
    /// <param name="count"></param>
    /// <param name="paramModel"></param>
    public void TriggerBuffMomentByCount(int count, MomentParamModel paramModel)
    {
        if (CanTriggerBuffEffect())
        {
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.TriggerEffectMomentID)
            {
                BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount * count, BattleMomentType.TriggerBuffMoment);
            }

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
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.TriggerEffectMomentID)
            {
                BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, count, BattleMomentType.TriggerBuffMoment);
            }

            OnTriggerBuffMomentByCountIgnoreLayerCount(count, paramModel);
        }
    }
    
    protected virtual void OnTriggerBuffMomentByCountIgnoreLayerCount(int count, MomentParamModel paramModel) {}

    public virtual (float, float) ChangeResourceCost(float gangQiCost, float xuanQiCost)
    {
        return (gangQiCost, xuanQiCost);
    }

    public virtual void ChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue, BattleSource source = BattleSource.None)
    {
        
    }

    public virtual void EndAction()
    {
        
    }

    public void SetTarget(BattleUnit target) => EffectTarget = target;
    
    public float AddSkillDamageRate(int skillGuid)
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnAddSkillDamageRate(skillGuid);
    }

    protected virtual float OnAddSkillDamageRate(int skillGuid) => 0;
    
    /// <summary>
    /// buff参与减少伤害的量
    /// </summary> dict => BuffMechanism, float  机制 改变伤害值
    /// <returns></returns>
    public virtual void ChangeDamageValue(Dictionary<int, float> dict, MomentParamModel paramModel) {}

    public virtual void KeyAdd(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
        
        OnKeyAdd(keyType, changeKeyData, reason);
    }

    protected virtual void OnKeyAdd(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason) { }
    
    public virtual void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
        
        OnKeyReduce(keyType, changeKeyData, reason);
    }

    protected virtual void OnKeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason) { }

    public virtual void BeAttack(float reduceHp, DamageType damageType, int attackID)
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

    #region Buff加的属性放在这里
    public float GetProperty(BattlePropertyType propertyType)
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetProperty(propertyType);
    }
    protected virtual float OnGetProperty(BattlePropertyType propertyType) => 0;
    
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
    public float GetAddWellyRate(int skillGuid)
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
    public float GetAddWellyEffect(int skillGuid)
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
    public void TrySetBaseWellyRate(int skillGuid, ref float value)
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
    public void TrySetAddWellyRate(int skillGuid, ref float value)
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

    public int GetKeyMax()
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetKeyPropertyMax();
    }
    protected virtual int OnGetKeyPropertyMax() => 0;

    public void SkillEnd()
    {
        OnSkillEnd();
    }

    protected virtual void OnSkillEnd() {}
}
