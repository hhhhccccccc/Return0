using System.Collections.Generic;
using cfg;

public interface IGetBattlePropertyChanged
{
    /// <summary>
    /// 获取威力改变
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <returns></returns>
    public float AddSkillWellyRate(int skillGuid);
    /// <summary>
    /// 获取威力改变效果
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <returns></returns>
    public float AddSkillWellyEffect(int skillGuid);
    /// <summary>
    /// 尝试设置基础威力
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <param name="value"></param>
    public void TrySetBaseWellyRate(int skillGuid, ref float value);
    /// <summary>
    /// 尝试设置威力增长
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <param name="value"></param>
    public void TrySetAddWellyRate(int skillGuid, ref float value);
    /// <summary>
    /// 获取键额外最大值
    /// </summary>
    public int GetKeyMaxEx();
    /// <summary>
    /// 血量改变时
    /// </summary>
    public void HpChanged();
    /// <summary>
    /// 技能结束时
    /// </summary>
    public void SkillEnd(BattleSkillBase skill);
    /// <summary>
    /// 获取属性
    /// </summary>
    /// <param name="propertyType"></param>
    public float GetProperty(BattlePropertyType propertyType);
    /// <summary>
    /// 获取息改变值
    /// </summary>
    /// <returns></returns>
    public int GetChangeActionWheel();
    /// <summary>
    /// 获取百分比增伤害
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <returns></returns>
    public float AddSkillDamageRate(int skillGuid);
    /// <summary>
    /// 键增加时
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="changeKeyData"></param>
    /// <param name="reason"></param>
    public void KeyAdd(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason);
    /// <summary>
    /// 键减少时
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="changeKeyData"></param>
    /// <param name="reason"></param>
    public void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason);
    /// <summary>
    /// 被攻击时
    /// </summary>
    /// <param name="reduceHp"></param>
    /// <param name="damageType"></param>
    /// <param name="attackID"></param>
    public void ReduceHp(float reduceHp, DamageType damageType, int attackID);
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
    /// 即将扣血时
    /// </summary>
    /// <param name="reduceHp"></param>
    public void BeforeReduceHp(float reduceHp);
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
    /// 改变属性之后
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
}
