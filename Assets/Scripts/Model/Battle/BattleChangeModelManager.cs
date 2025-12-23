using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleChangeModelManager : IModel, IRecycle
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
    /// <summary>
    /// 携带的宝器
    /// </summary>
    public List<BattleTreasureBase> Treasures = new();
    public bool CheckHasMethod(int methodID) => HeartMethods.Any(m => m.HeartMethodID == methodID);

    public BattleHeartMethodBase GetHeartMethod(int methodID)
    {
        return HeartMethods.FirstOrDefault(m => m.HeartMethodID == methodID);
    }
    
    private List<IGetBattlePropertyChanged> TempBattlePropertyChanged = new();
    
    public List<IGetBattlePropertyChanged> GetBattlePropertyChanged()
    {
        TempBattlePropertyChanged.Clear();
        TempBattlePropertyChanged.AddRange(Treasures);
        TempBattlePropertyChanged.AddRange(HeartMethods);
        TempBattlePropertyChanged.AddRange(Buffs.GetListValue());
        return TempBattlePropertyChanged;
    }

    private BattleUnit Unit;
    
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
    public float GetChangeModelGetAddWellyRate(int skillGuid)
    {
        return GetBattlePropertyChanged().Sum(changeModel => changeModel.AddSkillWellyRate(skillGuid));
    }
    /// <summary>
    /// 获取威力效果
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <returns></returns>
    public float GetChangeModelGetAddWellyEffect(int skillGuid)
    {
        return GetBattlePropertyChanged().Sum(changeModel => changeModel.AddSkillWellyEffect(skillGuid));
    }
    /// <summary>
    /// 获取威力效果
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <param name="value"></param>
    public void ChangeModelTrySetBaseWellyRate(int skillGuid, ref float value)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.TrySetBaseWellyRate(skillGuid, ref value);
        }
    }
    /// <summary>
    /// 尝试设置威力增长
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <param name="value"></param>
    public void ChangeModelTrySetAddWellyRate(int skillGuid, ref float value)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.TrySetAddWellyRate(skillGuid, ref value);
        }
    }
    /// <summary>
    /// 获取键最大值
    /// </summary>
    /// <returns></returns>
    public int GetKeyPropertyMax()
    { 
        return GetBattlePropertyChanged().Sum(changeModel => changeModel.GetKeyMaxEx());
    }
    /// <summary>
    /// 血量改变式
    /// </summary>
    public void OnHpChanged()
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.HpChanged();
        }
    }
    /// <summary>
    /// 技能结束时
    /// </summary>
    /// <param name="skill"></param>
    public void ChangeModelTriggerSkillEnd(BattleSkillBase skill)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.SkillEnd(skill);
        }
    }
    /// <summary>
    /// 改变息值
    /// </summary>
    /// <returns></returns>
    public int GetChangeActionWheel()
    {
        var changeActionWheel = GetBattlePropertyChanged().Sum(changeModel => changeModel.GetChangeActionWheel());
        TrySetChangeActionWheel(ref changeActionWheel);
        return changeActionWheel;
    }
    /// <summary>
    /// 获取百分比增伤害
    /// </summary>
    /// <param name="skillGuid"></param>
    /// <returns></returns>
    public float AddSkillDamageRate(int skillGuid)
    {
        return GetBattlePropertyChanged().Sum(changeModel => changeModel.AddSkillDamageRate(skillGuid));
    }

    /// <summary>
    /// 键增加时
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="changeKeyData"></param>
    /// <param name="reason"></param>
    /// <param name="changeType"></param>
    public void KeyAdd(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.KeyAdd(keyType, changeKeyData, reason, changeType);
        }
    }

    /// <summary>
    /// 键减少时
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="changeKeyData"></param>
    /// <param name="reason"></param>
    /// <param name="changeType"></param>
    public void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.KeyReduce(keyType, changeKeyData, reason, changeType);
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
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.AfterChangeKey(changeKeyData, isAdd, reason, changeType);
        }
    }
    /// <summary>
    /// 血量减少时
    /// </summary>
    /// <param name="reduceHp"></param>
    /// <param name="damageType"></param>
    /// <param name="attackID"></param>
    public void ReduceHp(float reduceHp, DamageType damageType, int attackID)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.ReduceHp(reduceHp, damageType, attackID);
        }
    }
    /// <summary>
    /// 获取可以代替刚气消耗的值
    /// </summary>
    /// <returns></returns>
    public float GetReplaceSkillGangQiCost()
    {
        return GetBattlePropertyChanged().Sum(changeModel => changeModel.GetReplaceSkillGangQiCost());
    }
    /// <summary>
    /// 生效可以代替刚气消耗的值
    /// </summary>
    /// <returns></returns>
    public void EffectReplaceSkillGangQiCost(ref float gangQiDelta)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.EffectReplaceSkillGangQiCost(ref gangQiDelta);
        }
    }
    /// <summary>
    /// 获取可以代替玄气消耗的值
    /// </summary>
    /// <returns></returns>
    public float GetReplaceSkillXuanQiCost()
    {
        return GetBattlePropertyChanged().Sum(changeModel => changeModel.GetReplaceSkillXuanQiCost());
    }
    /// <summary>
    /// 生效可以代替玄气消耗的值
    /// </summary>
    /// <returns></returns>
    public void EffectReplaceSkillXuanQiCost(ref float xuanQiDelta)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.EffectReplaceSkillXuanQiCost(ref xuanQiDelta);
        }
    }
    /// <summary>
    /// 击杀目标
    /// </summary>
    /// <param name="beKillID"></param>
    public void OnKillUnit(int beKillID)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.OnKillUnit(beKillID);
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
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            (gangQiCost, xuanQiCost) = changeModel.ChangeResourceCost(gangQiCost, xuanQiCost);
        }

        return (gangQiCost, xuanQiCost);
    }

    /// <summary>
    /// 是否重新计算伤害
    /// </summary>
    /// <param name="model"></param>
    public bool CheckReCalculateDamage(MomentParamModel model)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            if (changeModel.CheckReCalculateDamage(model))
            {
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// 扣血前
    /// </summary>
    /// <param name="reduceHp"></param>
    public void BeforeReduceHp(float reduceHp)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.BeforeReduceHp(reduceHp);
        }
    }
    /// <summary>
    /// 键的代替
    /// </summary>
    /// <param name="result"></param>
    /// <param name="keyType"></param>
    public void KeyReplace(List<int> result, BattleKeyType keyType)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.KeyReplace(result, keyType);
        }
    }

    /// <summary>
    /// 转化获得的键
    /// </summary>
    /// <param name="keyType"></param>
    /// <param name="count"></param>
    public void ConvertChangeKey(ref BattleKeyType keyType, int count)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.ConvertChangeKey(ref keyType, count);
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
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.BeforeChangeProperty(pType, ref value, source);
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
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.AfterChangeProperty(propType, originPropValue, finalPropValue, source);
        }
    }
    /// <summary>
    /// 行动结束 在扣除行动次数之后调用
    /// </summary>
    public void EndAction()
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.EndAction();
        }
    }
    /// <summary>
    /// 移除下次行动前效果
    /// </summary>
    public void RemoveBeforeNextAction()
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.RemoveBeforeNextAction();
        }
    }
    /// <summary>
    /// buff层数改变时
    /// </summary>
    /// <param name="buffID"></param>
    /// <param name="layerCount"></param>
    public void BuffLayerCountChanged(int buffID, int layerCount)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.BuffLayerCountChanged(buffID, layerCount);
        }
    }
    /// <summary>
    /// 伤害改变整数变量
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="paramModel"></param>
    public void ChangeDamageValue(Dictionary<int, float> dict, MomentParamModel paramModel)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.ChangeDamageValue(dict, paramModel);
        }
    }
    #endregion
    /// <summary>
    /// 初始化之后
    /// </summary>
    public void AfterUnitInit()
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.AfterUnitInit();
        }
    }
    /// <summary>
    /// 尝试设置改变息
    /// </summary>
    private void TrySetChangeActionWheel(ref int changeActionWheel)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.TrySetChangeActionWheel(ref changeActionWheel);
        }
    }
    /// <summary>
    /// 尝试设置改变息
    /// </summary>
    public void BeCounter()
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.BeCounter();
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
            foreach (var changeModel in GetBattlePropertyChanged())
            {
                if (!state)
                {
                    changeModel.ReCheckClashState(ref state, subjectDamageRate, targetDamageRate);
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
    public bool CheckCanAddBuff(int buffID, ref int addCount, int spellCasterID, BattleMomentType momentType = BattleMomentType.None)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            if (!changeModel.CheckCanAddBuff(buffID, ref addCount, spellCasterID, momentType))
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
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            changeModel.AfterGetProperty(propertyType, ref value, model);
        }
    }

    public bool CanBeCounter(MomentParamModel paramModel)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            if (!changeModel.CanBeCounter(paramModel))
            {
                return false;
            }
        }

        return true;
    }
    #region 加上技能的
    
    /// <summary>
    /// 判断是否能豁免直接杀式伤害
    /// </summary>
    /// <returns></returns>
    public bool CanIgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        foreach (var changeModel in GetBattlePropertyChanged())
        {
            if (changeModel.CanIgnoreSkillDirectDamage(paramModel))
            {
                return true;
            }
        }

        var skill = Unit.GetSkill();
        if (skill != null)
        {
            if (skill.CanIgnoreSkillDirectDamage())
            {
                return true;
            }
        }

        return false;
    }

    #endregion
}
