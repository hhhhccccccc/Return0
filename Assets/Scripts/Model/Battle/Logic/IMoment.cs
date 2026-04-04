using System.Collections.Generic;
using cfg;

public interface IMoment
{
    #region 扳机

    /// <summary>
    /// 战斗开始时
    /// </summary>
    public void BattleStart();
    /// <summary>
    /// 回合开始时
    /// </summary>
    public void RoundStart();
    /// <summary>
    /// 计算息
    /// </summary>
    public void CalculateActionWheel();
    /// <summary>
    /// 决定行动前调用
    /// </summary>
    public void BeforeDoDesitionAction();
    /// <summary>
    /// 决定行动的调用
    /// </summary>
    public void DoDesitionAction(bool isPreDesition);
    /// <summary>
    /// 每一息行动开始的调用
    /// </summary>
    public void EveryActionWheelStart();
    /// <summary>
    /// 自己息行动的开始的调用
    /// </summary>
    public void SelfActionWheelStart();
    /// <summary>
    /// 行动前
    /// </summary>
    public void BeforeAction();
    /// <summary>
    /// 受到行动前调用
    /// </summary>
    public void BeforeUnderAction();
    /// <summary>
    /// 交锋前
    /// </summary>
    public void BeforeClash(MomentParamModel paramModel);
    /// <summary>
    /// 交锋后
    /// </summary>
    public void AfterClash(MomentParamModel paramModel);
    /// <summary>
    /// 技能释放成功
    /// </summary>
    public void ReleaseSkillAction(MomentParamModel paramModel);
    /// <summary>
    /// 受到行动后调用
    /// </summary>
    public void AfterUnderAction(MomentParamModel paramModel);
    /// <summary>
    /// 行动后 
    /// </summary>
    public void AfterAction(MomentParamModel paramModel);
    /// <summary>
    /// 息结束的调用
    /// </summary>
    public void ActionWheelEnd();
    /// <summary>
    /// 回合结束后
    /// </summary>
    public void RoundEnd();
    /// <summary>
    /// 战斗结束
    /// </summary>
    public void BattleEnd();

    #endregion
    
    public void EnqueueViewModel(BattleMomentViewModel viewModel);
    public BattleMomentViewModel AllocViewModel(int entityID, MomentViewType viewType);


    #region 效果

       
    /// <summary>
    /// 获取威力改变
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <returns></returns>
    public float GetWellyRateEx(int skillGuid);
    /// <summary>
    /// 获取威力改变效果
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <returns></returns>
    public float GetWellyIncrease(int skillGuid);
    /// <summary>
    /// 尝试设置基础威力
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <param name="value"></param>
    public void TrySetWellyRateBase(int skillGuid, ref float value);
    /// <summary>
    /// 尝试设置威力增长
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <param name="value"></param>
    public void TrySetWellyRateEx(int skillGuid, ref float value);
    /// <summary>
    /// 获取键额外最大值
    /// </summary>
    public int GetKeyMaxEx();
    /// <summary>
    /// 技能结束时
    /// </summary>
    public void SkillEnd(BattleSkillBase skill);
    /// <summary>
    /// 获取属性
    /// </summary>
    /// <param name="propertyType"></param>
    /// <param name="model"></param>
    public float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null);
    /// <summary>
    /// 获取属性之后
    /// </summary>
    /// <param name="propertyType"></param>
    /// <param name="value"></param>
    /// <param name="model"></param>
    public void AfterGetProperty(BattlePropertyType propertyType, ref float value, GetPropertySourceModel model = null);
    /// <summary>
    /// 获取息改变值
    /// </summary>
    /// <returns></returns>
    public int GetChangeActionWheel();
    /// <summary>
    /// 获取百分比增伤害
    /// </summary>
    /// <param name="paramModel"></param>
    /// <returns></returns>
    public float AttackDamageAddPct(MomentParamModel paramModel);
    /// <summary>
    /// 键增加时
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="changeKeyData"></param>
    /// <param name="reason"></param>
    /// <param name="changeType"></param>
    public void KeyAdd(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType);
    /// <summary>
    /// 键减少时
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="changeKeyData"></param>
    /// <param name="reason"></param>
    /// <param name="changeType"></param>
    public void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType);
    /// <summary>
    /// 改变键之后
    /// </summary>
    /// <param name="changeKeyData"></param>
    /// <param name="isAdd"></param>
    /// <param name="reason"></param>
    /// <param name="changeType"></param>
    public void AfterChangeKey(List<BattleKey> changeKeyData, bool isAdd, ChangeKeyReason reason, ChangeKeyType changeType);

    /// <summary>
    /// 被攻击时
    /// </summary>
    /// <param name="isReduce"></param>
    /// <param name="changeHp"></param>
    /// <param name="damageType"></param>
    /// <param name="attackID"></param>
    /// <param name="isReduceHpMax"></param>
    public void AfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax);
    /// <summary>
    /// 获取可以代替刚气消耗的值
    /// </summary>
    /// <returns></returns>
    public float GetReplaceSkillGangQiCost();
    /// <summary>
    /// 生效可以代替刚气消耗的值
    /// </summary>
    /// <returns></returns>
    public void EffectReplaceSkillGangQiCost(ref float gangQiDelta);
    /// <summary>
    /// 获取可以代替玄气消耗的值
    /// </summary>
    /// <returns></returns>
    public float GetReplaceSkillXuanQiCost();
    /// <summary>
    /// 生效可以代替玄气消耗的值
    /// </summary>
    /// <returns></returns>
    public void EffectReplaceSkillXuanQiCost(ref float xuanQiDelta);
    /// <summary>
    /// 击杀目标
    /// </summary>
    /// <param name="beKillID"></param>
    public void OnKillUnit(int beKillID);
    /// <summary>
    /// 改变技能气的消耗
    /// </summary>
    /// <param name="gangQiCost"></param>
    /// <param name="xuanQiCost"></param>
    /// <returns></returns>
    public (float, float) ChangeResourceCost(float gangQiCost, float xuanQiCost);
    /// <summary>
    /// 是否重新计算伤害
    /// </summary>
    /// <param name="model"></param>
    public bool CheckReCalculateDamage(MomentParamModel model);

    /// <summary>
    /// 血量变化前
    /// </summary>
    /// <param name="changeHp"></param>
    /// <param name="damageType"></param>
    /// <param name="attackID"></param>
    /// <param name="isReduceHpMax"></param>
    /// <param name="isReduce"></param>
    public void BeforeChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax);
    /// <summary>
    /// 键的代替
    /// </summary>
    /// <param name="result"></param>
    /// <param name="keyType"></param>
    public void KeyReplace(List<int> result, BattleKeyType keyType);
    /// <summary>
    /// 转化获得的键
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="count"></param>
    public void ConvertChangeKey(ref BattleKeyType keyType, int count);
    /// <summary>
    /// 改变属性之前
    /// </summary>
    /// <param name="pType"></param>
    /// <param name="value"></param>
    /// <param name="source"></param>
    public void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source);
    /// <summary>
    /// 改变属性之后
    /// </summary>
    /// <param name="propType"></param>
    /// <param name="originPropValue"></param>
    /// <param name="finalPropValue"></param>
    /// <param name="source"></param>
    public void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue, BattleSource source = BattleSource.None);
    /// <summary>
    /// 行动结束 在扣除行动次数之后调用
    /// </summary>
    public void EndAction();
    /// <summary>
    /// 移除下次行动前效果
    /// </summary>
    public void RemoveBeforeNextAction();
    /// <summary>
    /// buff层数改变时
    /// </summary>
    /// <param name="buffID"></param>
    /// <param name="layerCount"></param>
    public void BuffLayerCountChanged(int buffID, int layerCount);
    /// <summary>
    /// 攻击方伤害改变整数变量
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="paramModel"></param>
    public void AddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel);
    /// <summary>
    /// 受击方伤害改变整数变量
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="paramModel"></param>
    public void ReduceDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel);
    /// <summary>
    /// Unit初始化之后
    /// </summary>
    public void AfterUnitInit();
    /// <summary>
    /// 尝试设置改变息
    /// </summary>
    public void TrySetChangeActionWheel(ref int changeActionWheel);
    /// <summary>
    /// 被破招
    /// </summary>
    public void BeCounter();
    /// <summary>
    /// 尝试改判交锋结果
    /// </summary>
    /// <param name="state"></param>
    /// <param name="subjectDamageRate"></param>
    /// <param name="targetDamageRate"></param>
    public void ReCheckClashState(ref bool state, float subjectDamageRate, float targetDamageRate);
    /// <summary>
    /// 判断能不能上buff
    /// </summary>
    /// <param name="buffID"></param>
    /// <param name="addCount"></param>
    /// <param name="spellCasterID"></param>
    /// <param name="momentType"></param>
    /// <returns></returns>
    public bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType = BattleMomentType.None);
    /// <summary>
    /// 判断是否能抵挡直接杀式伤害
    /// </summary>
    /// <returns></returns>
    public bool CanIgnoreSkillDirectDamage(MomentParamModel paramModel);
    /// <summary>
    /// 是否可被破招
    /// </summary>
    /// <param name="paramModel"></param>
    /// <returns></returns>
    public bool CheckDontBeCounter(MomentParamModel paramModel);
    /// <summary>
    /// 获取伤害百分比减免
    /// </summary>
    /// <param name="attackID"></param>
    /// <param name="damageType"></param>
    /// <returns></returns>
    public float BeDamageReducePct(int attackID, DamageType damageType);
    /// <summary>
    /// 攻击前
    /// </summary>
    /// <param name="model"></param>
    public void BeforeAttack(MomentParamModel model);

    /// <summary>
    /// 被攻击后
    /// </summary>
    /// <param name="damageType"></param>
    public void BeDamage(DamageType damageType);
    /// <summary>
    /// 尝试存储键
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="count"></param>
    public void TryStoreBattleKey(BattleKeyType keyType, ref int count);
    /// <summary>
    /// 清理临时数据
    /// </summary>
    void ClearTempData();
    
    #endregion
    
}
