
using System.Collections.Generic;
using cfg;
using UnityEngine;
using Zenject;

public class BattleUtil : SingleModel
{
    [Inject] private ConfigManager ConfigManager;
    public SkillType GetSkillTypeBySkillID(int skillID)
    {
        var config = ConfigManager.GetBattleSkill(skillID);
        return (SkillType)config.SkillType;
    }
    
    public bool SkillIsKillingStyle(SkillType skillType)
    {
        return skillType == SkillType.ArtKilling || skillType == SkillType.PowerKilling;
    }
    
    public bool SkillIsKillingStyle(int skillID)
    {
        return SkillIsKillingStyle(GetSkillTypeBySkillID(skillID));
    }

    public BattlePropertyType GetSkillFirstKey(int skillID)
    {
        var config = ConfigManager.GetBattleSkill(skillID);
        return (BattlePropertyType)config.NeedKey[0];
    }

    public List<int> GetSkillNeedKey(int skillID)
    {
        var config = ConfigManager.GetBattleSkill(skillID);
        return config.NeedKey;
    }

    /// <summary>
    /// 比较
    /// </summary>
    /// <param name="value1">传入值</param>
    /// <param name="value2">需要比较的值</param>
    /// <param name="compare">比较关系</param>
    /// <returns></returns>
    public bool CompareValue(float hasValue, float checkValue, int compare)
    {
        return (compare == -2 && hasValue < checkValue)
               || (compare == -1 && hasValue <= checkValue)
               || (compare == 0 && Mathf.Approximately(hasValue, checkValue))
               || (compare == 1 && hasValue >= checkValue)
               || (compare == 2 && hasValue > checkValue);
    }
}
