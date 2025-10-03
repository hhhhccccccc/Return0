using System;
using System.Collections.Generic;
using Zenject;

public class BattleTypeManager : SingleModel
{
    [Inject] private ConfigManager ConfigManager { get; set; }
    private Dictionary<int, Type> SkillTypeDic = new();
    private Dictionary<int, Type> BuffTypeDic = new();
    
    public Type GetSkillType(int skillID)
    {
        if (!SkillTypeDic.TryGetValue(skillID, out var type))
        {
            var config = ConfigManager.GetBattleSkillConfig(skillID);
            if (config != null)
            {
                type = Type.GetType(config.SkillScript);
            }
            SkillTypeDic.Add(skillID, type);
        }

        return type;
    }
    
    public Type GetBuffType(int buffID)
    {
        if (!BuffTypeDic.TryGetValue(buffID, out var type))
        {
            var config = ConfigManager.GetBattleBuffConfig(buffID);
            if (config != null)
            {
                type = Type.GetType(config.Script);
            }
            BuffTypeDic.Add(buffID, type);
        }

        return type;
    }
}
