using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;
using ValueType = System.ValueType;

public class BattleMoment : IMoment, IAlloc, IRecycle
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
    [Inject] protected IPoolManager PM { get; set; }
    [Inject] protected BattleMomentManager BattleMomentManager { get; set; }
    [Inject] protected BattleRecordManager BattleRecordManager { get; set; }
    [Inject] protected IMessageManager MessageManager { get; set; }
    [Inject] protected ConfigHelper ConfigHelper { get; set; }
    [Inject] protected ConfigManager ConfigManager { get; set; }
    [Inject] protected BattleBuffManager BattleBuffManager { get; set; }
    [Inject] protected BattleManager BattleManager { get; set; }
    [Inject] protected BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] protected BattleLogicStateManager BattleLogicStateManager { get; set; }
    [Inject] protected BattleUtil BattleUtil { get; set; }
    [Inject] protected ILogManager LM { get; set; }
    protected BattleUnit Subject { get; set; }
    protected bool Valid { get; set; }
    public virtual void BattleStart()
    {
        
    }

    public virtual void RoundStart()
    {
       
    }

    public virtual void CalculateActionWheel()
    {
        
    }

    public virtual void BeforeDoDesitionAction()
    {
        
    }

    public virtual void DoDesitionAction(bool isPreDesition)
    {  
        
    }

    public virtual void EveryActionWheelStart()
    {
        
    }

    public virtual void SelfActionWheelStart()
    {
       
    }

    public virtual void BeforeAction()
    {  
        
    }
    
    public virtual void BeforeUnderAction()
    {  
        
    }

    public virtual void BeforeClash(MomentParamModel paramModel)
    {  
       
    }
    
    public virtual void AfterClash(MomentParamModel paramModel)
    {  
        
    }
    
    public virtual void ReleaseSkillAction(MomentParamModel paramModel)
    {   
        
    }
    public virtual void AfterUnderAction(MomentParamModel paramModel)
    {
        
    }
    
    public virtual void AfterAction(MomentParamModel paramModel)
    {   
        
    }

    public virtual void ActionWheelEnd()
    {
        
    }

    public virtual void RoundEnd()
    {
        
    }

    public virtual void BattleEnd()
    {
        
    }

    public virtual void EnqueueViewModel(BattleMomentViewModel viewModel)
    {
        BattleRecordManager.AddBattleMomentViewModel(viewModel);
    }

    public virtual BattleMomentViewModel AllocViewModel(int entityID, MomentViewType viewType)
    {
        return null;
    }

    public virtual float GetSkillWelly(int skillGuid)
    {
        return 0;
    }

    public virtual float GetSkillWellyEffect(int skillGuid)
    {
        return 0;
    }

    public virtual void TrySetBaseWelly(int skillGuid, ref float value)
    {
        
    }

    public virtual void TrySetAddWelly(int skillGuid, ref float value)
    {
        
    }

    public virtual int GetKeyMaxEx()
    {
        return 0;
    }

    public virtual void SkillEnd(BattleSkillBase skill)
    {
        
    }

    public virtual float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        return 0;
    }

    public virtual void AfterGetProperty(BattlePropertyType propertyType, ref float value, GetPropertySourceModel model = null)
    {
        
    }

    public virtual int GetChangeActionWheel()
    {
        return 0;
    }

    public virtual float GetSkillDamageRate(MomentParamModel paramModel)
    {
        return 0;
    }

    public virtual void KeyAdd(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        
    }

    public virtual void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        
    }

    public virtual void AfterChangeKey(List<BattleKey> changeKeyData, bool isAdd, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        
    }

    public virtual void AfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        
    }

    public virtual float GetReplaceSkillGangQiCost()
    {
        return 0;
    }

    public virtual void EffectReplaceSkillGangQiCost(ref float gangQiDelta)
    {
        
    }

    public virtual float GetReplaceSkillXuanQiCost()
    {
        return 0;
    }

    public virtual void EffectReplaceSkillXuanQiCost(ref float xuanQiDelta)
    {
        
    }

    public virtual void OnKillUnit(int beKillID)
    {
        
    }

    public virtual (float, float) ChangeResourceCost(float gangQiCost, float xuanQiCost)
    {
        return (gangQiCost, xuanQiCost);
    }

    public virtual bool CheckReCalculateDamage(MomentParamModel model)
    {
        return false;
    }

    public virtual void BeforeChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        
    }

    public virtual void KeyReplace(List<int> result, BattleKeyType keyType)
    {
        
    }

    public virtual void ConvertChangeKey(ref BattleKeyType keyType, int count)
    {
        
    }

    public virtual void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        
    }

    public virtual void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
        BattleSource source = BattleSource.None)
    {
        
    }

    public virtual void EndAction()
    {
        
    }

    public virtual void RemoveBeforeNextAction()
    {
        
    }

    public virtual void BuffLayerCountChanged(int buffID, int layerCount)
    {
        
    }

    public virtual void AddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        
    }

    public virtual void ReduceDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        
    }

    public virtual void AfterUnitInit()
    {
        
    }

    public virtual void TrySetChangeActionWheel(ref int changeActionWheel)
    {
        
    }

    public virtual void BeCounter()
    {
        
    }

    public virtual void ReCheckClashState(ref bool state, float subjectDamageRate, float targetDamageRate)
    {
        
    }

    public virtual bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID,
        BattleMomentType momentType = BattleMomentType.None)
    {
        return true;
    }

    public virtual bool CanIgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        return false;
    }

    public virtual bool CanBeCounter(MomentParamModel paramModel)
    {
        return true;
    }

    public virtual float GetDamageReducePct(int attackID, DamageType damageType)
    {
        return 0;
    }

    public virtual void BeforeAttack(MomentParamModel model)
    {
        
    }

    public virtual void BeDamage(DamageType damageType)
    {
        
    }

    public virtual void TryStoreBattleKey(BattleKeyType keyType, ref int count)
    {
        
    }

    public virtual void Alloc()
    {
        
    }

    public void Recycle()
    {
        foreach (var disposable in _registerDisposables)
        {
            disposable.Dispose();
        }
        
        _registerDisposables.Clear();
        OnRecycle();
    }
    
    protected virtual void OnRecycle() {}
    
    #region 常用Effect执行方法
    

    /// <summary>
    /// 添加行动次数
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="times">次数</param>
    protected void DoAddActionTimes(BattleUnit target, int times)
    {
        if (target == null) return;
        target.AddActionTimes(times);
    }

    /// <summary>
    /// 移除所有键并添加各种键
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="changeKeyReason"></param>
    /// <param name="changeKeyType"></param>
    protected List<BattleKey> DoRemoveAllKey(BattleUnit target, ChangeKeyReason changeKeyReason, ChangeKeyType changeKeyType)
    {
        return target.RemoveAllKey(changeKeyReason, changeKeyType);
    }

    /// <summary>
    /// 添加所有键各几个
    /// </summary>
    /// <param name="target"></param>
    /// <param name="count"></param>
    /// <param name="changeKeyReason"></param>
    /// <param name="changeKeyType"></param>
    protected void DoAddAllKey(BattleUnit target, int count, ChangeKeyReason changeKeyReason, ChangeKeyType changeKeyType)
    {
        var list = new List<BattleKeyType>();
        for (int i = 1; i <= count; i++)
        {
            list.Add(BattleKeyType.KeyUp);
            list.Add(BattleKeyType.KeyDown);
            list.Add(BattleKeyType.KeyLeft);
            list.Add(BattleKeyType.KeyRight);
        }
        target.ChangeKeyList(list, true, changeKeyReason, changeKeyType);
    }
    
    protected void DoSetProperty(BattleUnit unit, BattlePropertyType type, int value, BattleSource source)
    {
        unit.SetProperty(type, value, source);
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

    /// <summary>
    /// 添加随机键到某个值
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="count"></param>
    protected void DoAddRandomKeyToDefineCount(BattleUnit unit, int count)
    {
        if (count == 0)
        {
            count = unit.GetKeyPropertyMax();
        }
        var has = unit.GetAllKeyCount();
        if (has >= count) return;
        var addCount = has - count;
        var list = Util.GetRandomKey(addCount);
        Subject.ChangeKeyList(list, true, ChangeKeyReason.SkillEffect);
    }

    /// <summary>
    /// 清理某类buff
    /// </summary>
    /// <param name="subject"></param>
    /// <param name="removeType"></param>
    /// <param name="removeCount"></param>
    protected void DoClearBuffByType(BattleUnit unit, BuffType removeType, int removeCount = 0)
    {
        var badBuffList = unit.GetRandomBuffByType(removeType, removeCount);
        foreach (var badBuff in badBuffList)
        {
            unit.ClearBuff(badBuff.BuffID);
        }
    }
    
    /// <summary>
    /// 设置目标到当前息
    /// </summary>
    /// <param name="unit">目标单位</param>
    protected void DoSetActionWheelToNow(BattleUnit unit)
    {
        if (unit == null) return;
        unit.SetActionWheelToNow();
        BattleLogicStateManager.CallAddUnitToNowLogicCalculate(unit.EntityID);
    }

    /// <summary>
    /// 若与杀式交锋则敌手因招式效果获得的炁-100
    /// </summary>
    protected void DoReduceHealQi(BattleUnit unit, BattleMomentType momentType)
    {
        DoAddBuff(unit, 90007, Subject, 1, null, momentType);
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
    /// 添加键
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="keyTypeList"></param>
    /// <param name="changeKeyReason"></param>
    /// <param name="changeKeyType"></param>
    protected void DoAddKey(BattleUnit unit, List<BattleKeyType> keyTypeList, ChangeKeyReason changeKeyReason, ChangeKeyType changeKeyType)
    {
        if (keyTypeList.Count <= 0)
        {
            return;
        }

        unit.ChangeKeyList(keyTypeList, true, changeKeyReason, changeKeyType);
    }

    /// <summary>
    /// 恢复属性（刚气/玄气）
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="propertyType">属性类型</param>
    /// <param name="value">恢复值</param>
    /// <param name="source"></param>
    protected float DoChangeProperty(BattleUnit target, BattlePropertyType propertyType, float value, BattleSource source)
    {
        return target.ChangeProperty(propertyType, value, source);
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
    /// 设置技能刚炁消耗
    /// </summary>
    /// <param name="target">目标单位</param>
    /// <param name="pct">当前刚炁百分比</param>
    /// <param name="maxCost">最大消耗上限</param>
    protected void DoChangeSkillGangQiCost(BattleUnit target, float pct, float maxCost)
    {
        var skill = target.GetSkill();
        if (skill != null)
        {
            var curr = target.GetProperty(BattlePropertyType.GangQi);
            var cost = curr * pct;
            if (maxCost > 0)
            {
                cost = Math.Min(cost, maxCost);
            }
            skill.SetGangQiCost(cost);
        }
    }

    /// <summary>
    /// 移除指定Buff
    /// </summary>
    /// <param name="unit">目标单位</param>
    /// <param name="buffID">BuffID</param>
    /// <param name="removeCount"></param>
    protected void DoRemoveBuff(BattleUnit unit, int buffID, int removeCount)
    {
        if (unit == null) return;
        var buffs = unit.GetBuffList();
        foreach (var buff in buffs)
        {
            if (buff.BuffID == buffID)
            {
                if (removeCount == 0)
                {
                    unit.ClearBuff(buffID);
                }
                else
                {
                    unit.ReduceBuffLayerCount(buffID, removeCount);
                }
                break;
            }
        }
    }
    
    /// <summary>
    /// 根据buffID 1比Count获取PoolID的buff
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="buffID"></param>
    /// <param name="count"></param>
    /// <param name="poolID"></param>
    /// <param name="momentType"></param>
    protected void AddGainBuffByBuffIDCount(BattleUnit unit, int buffID, int count, int poolID, BattleMomentType momentType)
    {
        var buffCount = unit.GetBuffCountByID(buffID);
        buffCount *= count;
        for (int i = 0; i < buffCount; i++)
        {
            var randomCount = GameConst.Battle.MaxRandomCount;
            while (randomCount > 0)
            {
                randomCount--;
                var poolResult = ConfigHelper.RandomCommonPool(poolID);
                var newBuffID = poolResult[0].ID;
                var newBuffLayerCount = poolResult[0].Num;
                var originBuff = unit.GetBuff(newBuffID);
                if (originBuff == null || !originBuff.IsMaxLayer())
                {
                    BattleBuffManager.AddBuff(unit, newBuffID, unit, newBuffLayerCount, null, momentType);
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 转换异常Buff为增益Buff
    /// </summary>
    /// <param name="unit">目标单位</param>
    /// <param name="poolID">增益Buff池ID</param>
    /// <param name="convertCount">转换数量</param>
    /// <param name="momentType"></param>
    protected void DoConvertBuffAbnormalToGain(BattleUnit unit, int poolID, int convertCount, BattleMomentType momentType)
    {
        var clearBuffList = unit.GetRandomBuffByType(BuffType.Abnormal, convertCount);
        var clearCount = clearBuffList.Count;
        foreach (var buff in clearBuffList)
        {
            unit.ClearBuff(buff.BuffID);
        }

        for (int i = 1; i <= clearCount; i++)
        {
            var randomCount = GameConst.Battle.MaxRandomCount;
            while (randomCount > 0)
            {
                randomCount--;
                var poolResult = ConfigHelper.RandomCommonPool(poolID);
                var newBuffID = poolResult[0].ID;
                var newBuffLayerCount = poolResult[0].Num;
                var originBuff = unit.GetBuff(newBuffID);
                if (originBuff == null || !originBuff.IsMaxLayer())
                {
                    BattleBuffManager.AddBuff(unit, newBuffID, unit, newBuffLayerCount, null, momentType);
                    break;
                }
            }
        }
    }
    
    /// <summary>
    /// 清除自身n个异常状态，若清除数量不超过n个则每少1个给予1层Pool
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="poolID"></param>
    /// <param name="removeCount"></param>
    /// <param name="momentType"></param>
    protected void DoClearAbnormalBuffAndAddGainBuff(BattleUnit unit, int poolID, int removeCount, BattleMomentType momentType)
    {
        var badBuffList = unit.GetRandomBuffByType(BuffType.Abnormal, removeCount);
        var removeSuccess = 0;
        foreach (var badBuff in badBuffList)
        {
            if (unit.ClearBuff(badBuff.BuffID))
            {
                removeSuccess++;
            }
        }

        if (removeSuccess > 0)
        {
            var buffDataList = ConfigHelper.RandomCommonPool(poolID);
            foreach (var buffData in buffDataList)
            {
                BattleBuffManager.AddBuff(unit, buffData.ID, unit, buffData.Num * removeSuccess, null, momentType);
            }
        }
    }

    protected bool CheckBeActionInBeforeActionWheel(BattleUnit unit, int actionWheel, bool includeNow)
    {
        var targetWheel = unit.ActionWheel;
        var now = BattleLogicStateManager.ActionWheel;
        if (includeNow)
        {
            return now + actionWheel >= targetWheel;
        }

        return now + actionWheel > targetWheel;
    }
    
    #endregion

    #region 检测
    
    /// <summary>
    /// 检查自己技能期间是否被打了（条件100001）
    /// </summary>
    /// <returns></returns>
    protected bool CheckBeDamageInSkillAction(BattleUnit unit)
    {
        return unit != null && unit.GetSkill()?.BeDirectDamageInSkillAction == true;
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

    protected bool CheckSkillIsKillingStyle(BattleUnit unit, bool isKillingStyle)
    { 
        var skillID = unit.GetSkillID();
        if (skillID != 0)
        {
            if (isKillingStyle)
            {
                return BattleUtil.SkillIsKillingStyle(skillID);
            }
            else
            {
                return !BattleUtil.SkillIsKillingStyle(skillID);
            }
        }

        return false;
    }

    protected bool CheckMutualGoal(BattleUnit self, BattleUnit other)
    {
        var selfSkill = self.GetSkill();
        var otherSkill = other.GetSkill();
        if (selfSkill != null && otherSkill != null)
        {
            return selfSkill.Target == other && otherSkill.Target == self;
        }

        return false;
    }

    protected bool CheckSelfIsOppoTarget(bool state)
    {
        var aliveUnitList = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
        if (state)
        {
            return aliveUnitList.Any(o =>
            {
                var skill = o.GetSkill();
                if (skill != null)
                {
                    return skill.Target.EntityID == Subject.EntityID;
                }

                return false;
            });
        }
        else
        {
            return aliveUnitList.All(o =>
            {
                var skill = o.GetSkill();
                if (skill != null)
                {
                    return skill.Target.EntityID != Subject.EntityID;
                }

                return true;
            });
        }
    }

    protected bool CheckProperty(BattleUnit unit, BattlePropertyType propertyType, DataType dataType, float value, DataRelation relation)
    {
        float hasValue;
        if (dataType == DataType.Int)//值
        {
            hasValue = unit.GetProperty(propertyType);
        }
        else if (dataType == DataType.Pct)//百分比
        {
            hasValue = unit.GetPropertyPct(propertyType);
        }
        else
        {
            return false;
        }
        
        return BattleUtil.CompareValue(hasValue, value, relation);
    }

    /// <summary>
    /// 键的数量判断
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="checkCount"></param>
    /// <param name="relation"></param>
    /// <returns></returns>
    protected bool CheckKeyCount(BattleUnit unit, int checkCount, DataRelation relation)
    {
        var hasCount = unit.GetAllKeyCount();
        return BattleUtil.CompareValue(hasCount, checkCount, relation);
    }

    /// <summary>
    /// 上次该技能是否被破招
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="relation"></param>
    /// <returns></returns>
    protected bool CheckLastUseSkillIsBeCounter(BattleUnit unit, bool relation)
    {
        var skill = unit.GetSkill();
        if (skill == null)
        {
            return false;
        }
        var state = unit.PreUseSkillDataManager.GetLastUseSkillState(skill.SkillGuid);

        if (relation)
        {
            return state == LastUseSkillState.BeCounter;
        }

        return state != LastUseSkillState.BeCounter;
    }

    /// <summary>
    /// buff数量判断
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="buffType"></param>
    /// <param name="checkCount"></param>
    /// <param name="relation"></param>
    /// <returns></returns>
    protected bool CheckBuffTypeCount(BattleUnit unit, BuffType buffType, int checkCount, DataRelation relation)
    {
        var hasCount = unit.GetBuffList().Count(buff => buff.BuffType == buffType);
        return BattleUtil.CompareValue(hasCount, checkCount, relation);
    }

    /// <summary>
    /// 本回合受到直接伤害次数判断
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="checkCount"></param>
    /// <param name="relation"></param>
    /// <returns></returns>
    protected bool CheckRoundBeDirectDamageTimes(BattleUnit unit, int checkCount, DataRelation relation)
    {
        var hasCount = unit.RoundBeDirectDamageTimes;
        return BattleUtil.CompareValue(hasCount, checkCount, relation);
    }
    
    #endregion
}