using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleMomentManager : IModel, IRecycle
{
    [Inject] private IPoolManager PoolManager { get; set; }
    /// <summary>
    /// 携带的buff
    /// </summary>
    public DictAndList<int, BattleBuffBase> Buffs = new();
    /// <summary>
    /// 携带的心法
    /// </summary>
    public List<BattleHeartMethodBase> HeartMethods = new();
    public bool CheckHasMethod(int methodID) => HeartMethods.Any(m => m.HeartMethodID == methodID);
    /// <summary>
    /// 携带的宝器
    /// </summary>
    public List<BattleTreasureBase> Treasures = new();
    public BattleTreasureBase GetTreasureByFeature(TreasureFeature feature)
    {
        foreach (var treasure in Treasures)
        {
            if (treasure.Config.Feature == (int)feature)
            {
                return treasure;
            }
        }

        return null;
    }

    public BattleHeartMethodBase GetHeartMethod(int methodID)
    {
        return HeartMethods.FirstOrDefault(m => m.HeartMethodID == methodID);
    }
    
    public List<IMoment> GetMoments(bool isLastSkill = true)
    {
        var list = new List<IMoment>();
        list.Clear();
        list.AddRange(Treasures);
        list.AddRange(HeartMethods);
        list.AddRange(Buffs.GetListValue());
        if (isLastSkill)
        {
            var skill = Unit.GetSkill();
            if (skill != null)
            {
                list.Add(skill);
                if (skill.Variant != null)
                {
                    list.Add(skill.Variant);
                }
            }
        }
        else
        {
            if (Unit.SkillSequence.Any())
            {
                var skill = Unit.SkillSequence.Last();
                list.Add(skill);
                if (skill.Variant != null)
                {
                    list.Add(skill.Variant);
                }
            }
        }
        
        return list;
    }

    private BattleUnit Unit { get; set; }
    
    public void Init(BattleUnit unit, HeroData heroData)
    {
        Unit = unit;
        foreach (var heartMethodID in heroData.WearHeartMethodList)
        {
            var heartMethod = PoolManager.GetClass<BattleHeartMethodBase>();
            heartMethod.Init(heartMethodID, unit);
            HeartMethods.Add(heartMethod);
        }
        foreach (var treasureID in heroData.WearTreasureList)
        {
            var treasure = PoolManager.GetClass<BattleTreasureBase>();
            treasure.Init(treasureID, unit);
            Treasures.Add(treasure);
        }
    }
    
    public void Recycle()
    {
        foreach (var buff in Buffs.GetListValue())
        {
            PoolManager.RecycleClass(buff);
        }
        Buffs.Clear();
        
        foreach (var heartMethodBase in HeartMethods)
        {
            PoolManager.RecycleClass(heartMethodBase);
        }
        HeartMethods.Clear();

        foreach (var treasureBase in Treasures)
        {
            PoolManager.RecycleClass(treasureBase);
        }
        Treasures.Clear();
    }

    #region 状态改变
    /// <summary>
    /// 获取威力改变
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <returns></returns>
    public float GetWellyRateExSum(int skillGuid)
    {
        return GetMoments().Sum(moment => moment.GetWellyRateEx(skillGuid));
    }
    /// <summary>
    /// 获取威力效果
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <returns></returns>
    public float GetWellyIncreaseSum(int skillGuid)
    {
        return GetMoments().Sum(moment => moment.GetWellyIncrease(skillGuid));
    }
    /// <summary>
    /// 尝试设置威力 基础威力
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <param name="value"></param>
    public void TrySetBaseWellyRate(int skillGuid, ref float value)
    {
        foreach (var moment in GetMoments())
        {
            moment.TrySetWellyRateBase(skillGuid, ref value);
        }
    }
    /// <summary>
    /// 尝试设置威力 额外威力
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <param name="value"></param>
    public void TrySetWellyRateEx(int skillGuid, ref float value)
    {
        foreach (var moment in GetMoments())
        {
            moment.TrySetWellyRateEx(skillGuid, ref value);
        }
    }
    /// <summary>
    /// 获取键最大值
    /// </summary>
    /// <returns></returns>
    public int GetKeyPropertyMax()
    { 
        return GetMoments().Sum(moment => moment.GetKeyMaxEx());
    }
    
    /// <summary>
    /// 技能结束时
    /// </summary>
    /// <param name="skill"></param>
    public void TriggerSkillEnd(BattleSkillBase skill)
    {
        foreach (var moment in GetMoments())
        {
            moment.SkillEnd(skill);
        }
    }
    /// <summary>
    /// 改变息值
    /// </summary>
    /// <returns></returns>
    public int GetChangeActionWheel()
    {
        var changeActionWheel = GetMoments().Sum(moment => moment.GetChangeActionWheel());
        TrySetChangeActionWheel(ref changeActionWheel);
        return changeActionWheel;
    }

    /// <summary>
    /// 获取百分比增伤害
    /// </summary>
    /// <param name="paramModel"></param>
    /// <returns></returns>
    public float AddDamagePct(MomentParamModel paramModel)
    {
        return GetMoments().Sum(moment => moment.AddDamagePct(paramModel));
    }

    /// <summary>
    /// 键增加时
    /// </summary>
    /// <param name="changeKeyData"></param>
    /// <param name="reason"></param>
    /// <param name="changeType"></param>
    public void KeyAdd(List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        foreach (var moment in GetMoments())
        {
            moment.KeyAdd(changeKeyData, reason, changeType);
        }
    }

    /// <summary>
    /// 键减少时
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="changeKeyData"></param>
    /// <param name="reason"></param>
    /// <param name="changeType"></param>
    public void KeyReduce(List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        foreach (var moment in GetMoments())
        {
            moment.KeyReduce(changeKeyData, reason, changeType);
        }
    }

    /// <summary>
    /// 改变键之后
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="changeKeyData"></param>
    /// <param name="isAdd"></param>
    /// <param name="reason"></param>
    /// <param name="changeType"></param>
    public void AfterChangeKey(List<BattleKey> changeKeyData, bool isAdd, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        foreach (var moment in GetMoments())
        {
            moment.AfterChangeKey(changeKeyData, isAdd, reason, changeType);
        }
    }

    /// <summary>
    /// 血量变化后
    /// </summary>
    /// <param name="isReduceHp"></param>
    /// <param name="reduceHp"></param>
    /// <param name="damageType"></param>
    /// <param name="attackID"></param>
    /// <param name="isReduceHpMax"></param>
    public void AfterReduceHp(bool isReduceHp, float reduceHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        foreach (var moment in GetMoments())
        {
            moment.AfterChangeHp(isReduceHp, reduceHp, damageType, attackID, isReduceHpMax);
        }
    }
    /// <summary>
    /// 获取可以代替刚气消耗的值
    /// </summary>
    /// <returns></returns>
    public float GetReplaceSkillGangQiCost()
    {
        return GetMoments().Sum(moment => moment.GetReplaceSkillGangQiCost());
    }
    /// <summary>
    /// 生效可以代替刚气消耗的值
    /// </summary>
    /// <returns></returns>
    public void EffectReplaceSkillGangQiCost(ref float gangQiDelta)
    {
        foreach (var moment in GetMoments())
        {
            moment.EffectReplaceSkillGangQiCost(ref gangQiDelta);
        }
    }
    /// <summary>
    /// 获取可以代替玄气消耗的值
    /// </summary>
    /// <returns></returns>
    public float GetReplaceSkillXuanQiCost()
    {
        return GetMoments().Sum(moment => moment.GetReplaceSkillXuanQiCost());
    }
    /// <summary>
    /// 生效可以代替玄气消耗的值
    /// </summary>
    /// <returns></returns>
    public void EffectReplaceSkillXuanQiCost(ref float xuanQiDelta)
    {
        foreach (var moment in GetMoments())
        {
            moment.EffectReplaceSkillXuanQiCost(ref xuanQiDelta);
        }
    }
    /// <summary>
    /// 击杀目标
    /// </summary>
    /// <param name="beKillID"></param>
    public void OnKillUnit(int beKillID)
    {
        foreach (var moment in GetMoments())
        {
            moment.OnKillUnit(beKillID);
        }
    }
    /// <summary>
    /// 改变技能气的消耗
    /// </summary>
    /// <param name="gangQiCost"></param>
    /// <param name="xuanQiCost"></param>
    /// <returns></returns>
    public (float, float) ChangeResourceCost(float gangQiCost, float xuanQiCost)
    {
        foreach (var moment in GetMoments())
        {
            (gangQiCost, xuanQiCost) = moment.ChangeResourceCost(gangQiCost, xuanQiCost);
        }

        return (gangQiCost, xuanQiCost);
    }

    /// <summary>
    /// 是否重新计算伤害
    /// </summary>
    /// <param name="model"></param>
    public bool CheckReCalculateDamage(MomentParamModel model)
    {
        foreach (var moment in GetMoments())
        {
            if (moment.CheckReCalculateDamage(model))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 是否能自然恢复气
    /// </summary>
    /// <param name="propertyType"></param>
    /// <returns></returns>
    public bool CheckCanRecoverNaturalQi(BattlePropertyType propertyType)
    {
        foreach (var moment in GetMoments())
        {
            if (!moment.CheckCanRecoverNaturalQi(propertyType))
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 是否能释放决定这个技能
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public bool CheckSkillCanDoDesition(int skillGuid, BattleUnit target)
    {
        foreach (var moment in GetMoments())
        {
            if (!moment.CheckSkillCanDoDesition(skillGuid, target))
            {
                return false;
            }
        }

        return true;
    }
    
    /// <summary>
    /// 血量变化前
    /// </summary>
    /// <param name="isReduceHp"></param>
    /// <param name="reduceHp"></param>
    /// <param name="damageType"></param>
    /// <param name="attackID"></param>
    /// <param name="isReduceHpMax"></param>
    public void BeforeReduceHp(bool isReduceHp, float reduceHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        foreach (var moment in GetMoments())
        {
            moment.BeforeChangeHp(isReduceHp, reduceHp, damageType, attackID, isReduceHpMax);
        }
    }
    /// <summary>
    /// 键的代替
    /// </summary>
    /// <param name="result"></param>
    /// <param name="keyType"></param>
    public void KeyReplace(List<int> result, BattleKeyType keyType)
    {
        foreach (var moment in GetMoments())
        {
            moment.KeyReplace(result, keyType);
        }
    }

    /// <summary>
    /// 转化获得的键
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="count"></param>
    public void ConvertChangeKey(ref BattleKeyType keyType, int count)
    {
        foreach (var moment in GetMoments())
        {
            moment.ConvertChangeKey(ref keyType, count);
        }
    }
    /// <summary>
    /// 改变属性之前
    /// </summary>
    /// <param name="pType"></param>
    /// <param name="value"></param>
    /// <param name="source"></param>
    public void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        foreach (var moment in GetMoments())
        {
            moment.BeforeChangeProperty(pType, ref value, source);
        }
    }
    /// <summary>
    /// 改变属性之后
    /// </summary>
    /// <param name="propType"></param>
    /// <param name="originPropValue"></param>
    /// <param name="finalPropValue"></param>
    /// <param name="source"></param>
    public void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue, BattleSource source = BattleSource.None)
    {
        foreach (var moment in GetMoments())
        {
            moment.AfterChangeProperty(propType, originPropValue, finalPropValue, source);
        }
    }
    /// <summary>
    /// 行动结束 在扣除行动次数之后调用
    /// </summary>
    public void EndAction()
    {
        foreach (var moment in GetMoments())
        {
            moment.EndAction();
        }
    }
    /// <summary>
    /// 移除下次行动前效果
    /// </summary>
    public void RemoveBeforeNextAction()
    {
        foreach (var moment in GetMoments())
        {
            moment.RemoveBeforeNextAction();
        }
    }
    /// <summary>
    /// buff层数改变时
    /// </summary>
    /// <param name="buffID"></param>
    /// <param name="layerCount"></param>
    public void BuffLayerCountChanged(int buffID, int layerCount)
    {
        foreach (var moment in GetMoments())
        {
            moment.BuffLayerCountChanged(buffID, layerCount);
        }
    }
    /// <summary>
    /// 攻击方伤害改变整数变量
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="paramModel"></param>
    public void AddDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        foreach (var moment in GetMoments())
        {
            moment.AddDamageValueInt(dict, paramModel);
        }
    }
    /// <summary>
    /// 受击方伤害改变整数变量
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="paramModel"></param>
    public void ReduceDamageValueInt(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        foreach (var moment in GetMoments())
        {
            moment.ReduceDamageInt(dict, paramModel);
        }
    }
    #endregion
    /// <summary>
    /// 初始化之后
    /// </summary>
    public void AfterUnitInit()
    {
        foreach (var moment in GetMoments())
        {
            moment.AfterUnitInit();
        }
    }
    /// <summary>
    /// 尝试设置改变息
    /// </summary>
    private void TrySetChangeActionWheel(ref int changeActionWheel)
    {
        foreach (var moment in GetMoments())
        {
            moment.TrySetChangeActionWheel(ref changeActionWheel);
        }
    }
    /// <summary>
    /// 尝试设置改变息
    /// </summary>
    public void BeCounter()
    {
        foreach (var moment in GetMoments())
        {
            moment.BeCounter();
        }
    }
    /// <summary>
    /// 尝试改判交锋结果
    /// </summary>
    /// <param name="state"></param>
    /// <param name="subjectDamageRate"></param>
    /// <param name="targetDamageRate"></param>
    public void ReCheckClashState(ref bool state, float subjectDamageRate, float targetDamageRate)
    {
        if (!state)
        {
            foreach (var moment in GetMoments())
            {
                if (!state)
                {
                    moment.ReCheckClashState(ref state, subjectDamageRate, targetDamageRate);
                }
            }
        }
    }

    /// <summary>
    /// 判断是否能添加buff
    /// </summary>
    /// <param name="buffID"></param>
    /// <param name="addCount"></param>
    /// <param name="spellCasterID"></param>
    /// <param name="momentType"></param>
    /// <returns></returns>
    public bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType)
    {
        foreach (var moment in GetMoments())
        {
            if (!moment.CheckCanAddBuff(buffID, ref addCount, spellCasterID, momentType))
            {
                return false;
            }
        }

        return true;
    }
    /// <summary>
    /// 获取属性后
    /// </summary>
    /// <param name="propertyType"></param>
    /// <param name="value"></param>
    /// <param name="model"></param>
    public void AfterGetProperty(BattlePropertyType propertyType, ref float value, GetPropertySourceModel model = null)
    {
        foreach (var moment in GetMoments())
        {
            moment.AfterGetProperty(propertyType, ref value, model);
        }
    }

    public bool CheckDontBeCounter(MomentParamModel paramModel)
    {
        foreach (var moment in GetMoments())
        {
            if (moment.CheckDontBeCounter(paramModel))
            {
                return true;
            }
        }

        return false;
    }

    public float BeDamageReducePct(int attackID, DamageType damageType)
    {
        return Math.Min(1, GetMoments().Sum(moment => moment.ReduceDamagePct(attackID, damageType))) ;
    }
    
    public void BeforeAttack(MomentParamModel model)
    {
        foreach (var moment in GetMoments())
        {
            moment.BeforeAttack(model);
        }
    }
    
    public void BeDamage(DamageType damageType)
    {
        foreach (var moment in GetMoments())
        {
            moment.BeDamage(damageType);
        }
    }
    public void TryStoreBattleKey(BattleKeyType keyType,ref int count)
    {
        foreach (var moment in GetMoments())
        {
            if (count > 0)
            {
                moment.TryStoreBattleKey(keyType, ref count);
            }
        }
    }
    
    public void ClearTempData()
    {
        foreach (var moment in GetMoments())
        {
            moment.ClearTempData();
        }
    }
    
    #region 加上技能的
    
    /// <summary>
    /// 判断是否能豁免直接杀式伤害
    /// </summary>
    /// <returns></returns>
    public bool IgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        foreach (var moment in GetMoments())
        {
            if (moment.IgnoreSkillDirectDamage(paramModel))
            {
                return true;
            }
        }
        
        return false;
    }

    public float GetMomentPropertySum(BattlePropertyType pType, GetPropertySourceModel model = null)
    {
        return GetMoments().Sum(moment => GetPropertyMomentBeEffect(moment, pType, model));
    } 
    
    private float GetPropertyMomentBeEffect(IMoment momentModel, BattlePropertyType pType, GetPropertySourceModel model = null)
    {
        var hasMethod10060 = Unit.BattleMomentManager.CheckHasMethod(GameConst.Battle.HeartMethod10060);
        if (hasMethod10060)
        {
            #region 心法10060影响
            
            //防变成力
            if (pType == BattlePropertyType.PowerInt)
            {
                var propertyA = momentModel.GetMomentProperty(BattlePropertyType.PowerInt, model);
                var propertyB =  momentModel.GetMomentProperty(BattlePropertyType.DefendInt, model);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.DefendInt)
            {
                var property = momentModel.GetMomentProperty(BattlePropertyType.DefendInt, model);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }
            
            if (pType == BattlePropertyType.PowerPct)
            {
                var propertyA = momentModel.GetMomentProperty(BattlePropertyType.PowerPct, model);
                var propertyB =  momentModel.GetMomentProperty(BattlePropertyType.DefendPct, model);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.DefendPct)
            {
                var property =  momentModel.GetMomentProperty(BattlePropertyType.DefendPct, model);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }
            
            if (pType == BattlePropertyType.AllPowerPct)
            {
                var propertyA = momentModel.GetMomentProperty(BattlePropertyType.AllPowerPct, model);
                var propertyB =  momentModel.GetMomentProperty(BattlePropertyType.AllDefendPct, model);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.AllDefendPct)
            {
                var property =  momentModel.GetMomentProperty(BattlePropertyType.AllDefendPct, model);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }
            
            if (pType == BattlePropertyType.PowerAddPct)
            {
                var propertyA = momentModel.GetMomentProperty(BattlePropertyType.PowerAddPct, model);
                var propertyB =  momentModel.GetMomentProperty(BattlePropertyType.DefendAddPct, model);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.DefendAddPct)
            {
                var property =  momentModel.GetMomentProperty(BattlePropertyType.DefendAddPct, model);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }
            
            //破变成技
            if (pType == BattlePropertyType.TechInt)
            {
                var propertyA = momentModel.GetMomentProperty(BattlePropertyType.TechInt, model);
                var propertyB =  momentModel.GetMomentProperty(BattlePropertyType.BreakInt, model);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.BreakInt)
            {
                var property = momentModel.GetMomentProperty(BattlePropertyType.BreakInt, model);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }
            
            if (pType == BattlePropertyType.TechPct)
            {
                var propertyA = momentModel.GetMomentProperty(BattlePropertyType.TechPct, model);
                var propertyB =  momentModel.GetMomentProperty(BattlePropertyType.BreakPct, model);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.BreakPct)
            {
                var property =  momentModel.GetMomentProperty(BattlePropertyType.BreakPct, model);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }
            
            if (pType == BattlePropertyType.AllTechPct)
            {
                var propertyA = momentModel.GetMomentProperty(BattlePropertyType.AllTechPct, model);
                var propertyB =  momentModel.GetMomentProperty(BattlePropertyType.AllBreakPct, model);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.AllBreakPct)
            {
                var property =  momentModel.GetMomentProperty(BattlePropertyType.AllBreakPct, model);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }
            
            if (pType == BattlePropertyType.TechAddPct)
            {
                var propertyA = momentModel.GetMomentProperty(BattlePropertyType.TechAddPct, model);
                var propertyB =  momentModel.GetMomentProperty(BattlePropertyType.BreakAddPct, model);
                if (propertyB >= 0)
                {
                    propertyA += propertyB;
                }

                return propertyA;
            }
            
            if (pType == BattlePropertyType.BreakAddPct)
            {
                var property =  momentModel.GetMomentProperty(BattlePropertyType.BreakAddPct, model);
                if (property >= 0)
                {
                    return 0;
                }

                return property;
            }

            #endregion
           
        }

        return momentModel.GetMomentProperty(pType, model);
    }

    #endregion
}
