using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuffBase : BattleMoment
{
    public int BuffID { get; private set; }
    public BuffType BuffType { get; private set; }
    public BattleBuffConfig Config { get; private set; }
    public BattleUnit SpellCaster { get; private set; }
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
        Subject.BattleMomentManager.BuffLayerCountChanged(BuffID, layerCount);
    }
    
    /// <summary>
    /// 减少buff持续时间
    /// </summary>
    protected virtual void ReduceLayerCountByMoment(BattleMomentType momentType, MomentParamModel paramModel = null)
    {
        if (!Valid)
        {
            return;
        }
        
        var reduceMoment =  Config.BuffLevelReduceMoment;
        for (int i = 0; i < reduceMoment.Count; i += 2)
        {
            var reduceMomentType = reduceMoment[i];
            if (reduceMomentType == (int)momentType)
            {
                ReduceLayer((BuffReduceType)reduceMoment[i + 1], paramModel);
            }
        }
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
        Subject.BattleMomentManager.BuffLayerCountChanged(BuffID, -layerCount);
        if (LayerCount <= 0)
        {
            Valid = false;
            OnBuffRemove();
            Subject.RemoveBuff(BuffID);
        }
    }

    protected bool CanTriggerBuffEffect()
    {
        if(!Valid)
        {
            return false;
        }
        
        if (BuffType == BuffType.Gain && Subject.HasBuffMechanism(BuffMechanism.NotBeAddGainBuff))
        {
            return false;
        }

        if (BuffType == BuffType.Abnormal && Subject.HasBuffMechanism(BuffMechanism.NotEffectAbnormalBuff))
        {
            return false;
        }

        return true;
    }
    
    protected virtual void OnBuffRemove()
    {
        
    }
    
    public virtual void ClearLayerCount()
    {
        ReduceLayerCount(LayerCount);
    }

    protected override void OnRecycle()
    {
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
        OnBuffRecycle();
    }
    
    protected virtual void OnBuffRecycle() {}

    public void SetTarget(BattleUnit target) => EffectTarget = target;

    
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
    
    public override float GetSkillDamageRate(MomentParamModel paramModel)
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnAddSkillDamageRate(paramModel);
    }

    protected virtual float OnAddSkillDamageRate(MomentParamModel paramModel) => 0;
    
    public override float GetDamageReducePct(int attackID, DamageType damageType)
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetDamageReducePct(attackID, damageType);
    }
    protected virtual float OnGetDamageReducePct(int attackID, DamageType damageType) => 0;

    public override void BeforeAttack(MomentParamModel model)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
    }

    public override void BeDamage(MomentParamModel model)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
    }

    public override void TryStoreBattleKey(BattleKeyType keyType, ref int count)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
    }
    
    public override void KeyAdd(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
        
        OnKeyAdd(keyType, changeKeyData, reason, changeType);
    }
    protected virtual void OnKeyAdd(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType) { }
    
    public override void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
        
        OnKeyReduce(keyType, changeKeyData, reason, changeType);
    }

    public override void AfterChangeKey(List<BattleKey> changeKeyData, bool isAdd, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        
    }

    protected virtual void OnKeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType) { }

    public override void AfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }

        OnAfterChangeHp(isReduce, changeHp, damageType, attackID, isReduceHpMax);
    }

    protected virtual void OnAfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        
    }
    
    public override float GetReplaceSkillGangQiCost()
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetReplaceSkillGangQiCost();
    }
    
    protected virtual float OnGetReplaceSkillGangQiCost() => 0;

    public override void EffectReplaceSkillGangQiCost(ref float gangQiDelta)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
        
        OnEffectReplaceSkillGangQiCost(ref gangQiDelta);
    }

    protected virtual void OnEffectReplaceSkillGangQiCost(ref float gangQiDelta) {}

    public override float GetReplaceSkillXuanQiCost()
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetReplaceSkillXuanQiCost();
    }
    private float OnGetReplaceSkillXuanQiCost() => 0;
    
    public override void EffectReplaceSkillXuanQiCost(ref float xuanQiDelta)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
        
        OnEffectReplaceSkillXuanQiCost(ref xuanQiDelta);
    }

    protected virtual void OnEffectReplaceSkillXuanQiCost(ref float xuanQiDelta) {}

    #region Buff加的属性放在这里
    public override float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetProperty(propertyType, model);
    }
    
    protected virtual float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null) => 0;
    
    #endregion

    #region 息改变量

    /// <summary>
    /// 加速或缓速
    /// </summary>
    /// <returns></returns>
    public override int GetChangeActionWheel()
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
    public override float GetSkillWelly(int skillGuid)
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
    public override float GetSkillWellyEffect(int skillGuid)
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
    public override void TrySetBaseWelly(int skillGuid, ref float value)
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
    public override void TrySetAddWelly(int skillGuid, ref float value)
    {
        if (!CanTriggerBuffEffect())
        {
            return;
        }
        
        OnTrySetAddWellyRate(skillGuid, ref value);
    }
    protected virtual void OnTrySetAddWellyRate(int skillGuid, ref float value) { }

    #endregion

    public override int GetKeyMaxEx()
    {
        if (!CanTriggerBuffEffect())
        {
            return 0;
        }

        return OnGetKeyPropertyMax();
    }
    protected virtual int OnGetKeyPropertyMax() => 0;
    

    public override void SkillEnd(BattleSkillBase skill)
    {
        OnSkillEnd(skill);
    }
    protected virtual void OnSkillEnd(BattleSkillBase skill) {}

    #region 扳机

    public override void BattleStart()
    {
        if (CanTriggerBuffEffect())
        {
            OnBattleStart();
        }
        ReduceLayerCountByMoment(BattleMomentType.BattleStart);
    }
    protected virtual void OnBattleStart() {}

    public override void RoundStart()
    {
        if (CanTriggerBuffEffect())
        {
            OnRoundStart();
        }
        ReduceLayerCountByMoment(BattleMomentType.RoundStart);
    }
    protected virtual void OnRoundStart() {}
    
    public override void CalculateActionWheel()
    {
        if (CanTriggerBuffEffect())
        {
            OnCalculateActionWheel();
        }
        ReduceLayerCountByMoment(BattleMomentType.CalculateActionWheel);
    }
    protected virtual void OnCalculateActionWheel() {}
    
    public override void BeforeDoDesitionAction()
    {
        if (CanTriggerBuffEffect())
        {
            OnBeforeDoDesitionAction();
        }
    }
    protected virtual void OnBeforeDoDesitionAction() { }
    
    public override void DoDesitionAction(bool isPreDesition)
    {  
        if (CanTriggerBuffEffect())
        {
            OnDoDesitionAction(isPreDesition);
        }
        ReduceLayerCountByMoment(BattleMomentType.DoDesitionAction);
    }
    protected virtual void OnDoDesitionAction(bool isPreDesition) {}
    
    public override void EveryActionWheelStart()
    {
        if (CanTriggerBuffEffect())
        {
            OnEveryActionWheelStart();
        }
        ReduceLayerCountByMoment(BattleMomentType.EveryActionWheelStart);
    }
    protected virtual void OnEveryActionWheelStart() {}

    public override void SelfActionWheelStart()
    {
        if (CanTriggerBuffEffect())
        {
            OnSelfActionWheelStart();
        }
        ReduceLayerCountByMoment(BattleMomentType.SelfActionWheelStart);
    }
    protected virtual void OnSelfActionWheelStart() {}

    public override void BeforeAction()
    {
        if (CanTriggerBuffEffect())
        {
            if (Subject.NotBeAbnormalBuffEffect > 0 && BuffType == BuffType.Abnormal)
            {
                return;
            }
            OnBeforeAction();
        }
        ReduceLayerCountByMoment(BattleMomentType.BeforeAction);
    }
    protected virtual void OnBeforeAction() {}

    public override void BeforeUnderAction()
    {  
        if (CanTriggerBuffEffect())
        {
            if (Subject.NotBeAbnormalBuffEffect > 0 && BuffType == BuffType.Abnormal)
            {
                return;
            }
            OnBeforeUnderAction();
        }
        ReduceLayerCountByMoment(BattleMomentType.BeforeUnderAction);
    }
    protected virtual void OnBeforeUnderAction(){}
    
    public override void BeforeClash(MomentParamModel paramModel)
    {  
        if (CanTriggerBuffEffect())
        {
            if (Subject.NotBeAbnormalBuffEffect > 0 && BuffType == BuffType.Abnormal)
            {
                return;
            }
            OnBeforeClash(paramModel);
        }
        ReduceLayerCountByMoment(BattleMomentType.BeforeClash);
    }
    protected virtual void OnBeforeClash(MomentParamModel paramModel) {}

    public override void AfterClash(MomentParamModel paramModel)
    {  
        if (CanTriggerBuffEffect())
        {
            if (Subject.NotBeAbnormalBuffEffect > 0 && BuffType == BuffType.Abnormal)
            {
                return;
            }
            OnAfterClash(paramModel);
        }
        ReduceLayerCountByMoment(BattleMomentType.AfterClash);
    }
    protected virtual void OnAfterClash(MomentParamModel paramModel) {}
    
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {  
        if (CanTriggerBuffEffect())
        {
            if (Subject.NotBeAbnormalBuffEffect > 0 && BuffType == BuffType.Abnormal)
            {
                return;
            }
            OnReleaseSkillAction(paramModel);
        }
        ReduceLayerCountByMoment(BattleMomentType.ReleaseSkillAction, paramModel);
    }
    protected virtual void OnReleaseSkillAction(MomentParamModel paramModel) {}
    
    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        if (CanTriggerBuffEffect())
        {
            if (Subject.NotBeAbnormalBuffEffect > 0 && BuffType == BuffType.Abnormal)
            {
                return;
            }
            OnAfterUnderAction(paramModel);
        }
       
        
        ReduceLayerCountByMoment(BattleMomentType.AfterUnderAction);
    }
    protected virtual void OnAfterUnderAction(MomentParamModel paramModel) {}

    public override void AfterAction(MomentParamModel paramModel)
    {  
        BeforeLastActionGetLayerCount = LayerCount;
        if (CanTriggerBuffEffect())
        {
            if (Subject.NotBeAbnormalBuffEffect > 0 && BuffType == BuffType.Abnormal)
            {
                return;
            }
            OnAfterAction(paramModel);
        }
        
        if (Config.BeStatusPersists == 1)
        {
            if (Subject.StatusPersists > 0)
            {
                return;
            }

            if (Subject.GainStatusPersists > 0 && BuffType == BuffType.Gain)
            {
                return;
            }
        }
        
        ReduceLayerCountByMoment(BattleMomentType.AfterAction);
    }
    protected virtual void OnAfterAction(MomentParamModel paramModel) {}

    public override void ActionWheelEnd()
    {
        if (CanTriggerBuffEffect())
        {
            OnActionWheelEnd();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.ActionWheelEnd);
    }
    protected virtual void OnActionWheelEnd() {}
    
    public override void RoundEnd()
    {  
        if (CanTriggerBuffEffect())
        {
            OnRoundEnd();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.RoundEnd);
    }
    
    protected virtual void OnRoundEnd() {}
    
    public override void BattleEnd()
    {
        if (CanTriggerBuffEffect())
        {
            OnBattleEnd();
        }
    }
    
    protected virtual void OnBattleEnd() {}

    #endregion
    
    public override void EnqueueViewModel(BattleMomentViewModel viewModel)
    {
        BattleRecordManager.AddBattleMomentViewModel(viewModel);
    }

    public override BattleMomentViewModel AllocViewModel(int entityID, MomentViewType viewType)
    {
        var viewModel = PM.GetClass<BattleMomentViewModel>();
        viewModel.BattleSource = BattleSource.Buff;
        viewModel.EntityID = entityID;
        viewModel.ConfigID = BuffID;
        return viewModel;
    }
}
