using System;
using System.Collections.Generic;
using Zenject;

public class BattleTypeManager : SingleModel
{
    [Inject] private ConfigManager ConfigManager { get; set; }
    private Dictionary<int, Type> SkillTypeDic = new();
    private Dictionary<int, Type> BuffTypeDic = new();
    private Dictionary<int, Type> HeartMethodTypeDic = new();
    private Dictionary<int, Type> TreasureTypeDic = new();
    
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
    
    public Type GetHeartMethodType(int heartMethodID)
    {
        if (!HeartMethodTypeDic.TryGetValue(heartMethodID, out var type))
        {
            var config = ConfigManager.GetHeartMethodConfig(heartMethodID);
            if (config != null)
            {
                type = Type.GetType(config.Script);
            }
            HeartMethodTypeDic.Add(heartMethodID, type);
        }

        return type;
    }
    
    public Type GetTreasureType(int treasureID)
    {
        if (!TreasureTypeDic.TryGetValue(treasureID, out var type))
        {
            var config = ConfigManager.GetTreasureConfig(treasureID);
            if (config != null)
            {
                type = Type.GetType(config.Script);
            }
            TreasureTypeDic.Add(treasureID, type);
        }

        return type;
    }
}
