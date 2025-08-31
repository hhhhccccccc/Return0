using System.Collections;
using System.Collections.Generic;
using System.IO;
using cfg;
using SimpleJSON;
using UnityEngine;

public class ConfigManager
{
    private Tables _tables;

    public IEnumerator OnInit()
    {
        string gameConfDir = Application.streamingAssetsPath + "/Luban"; // 替换为gen.bat中outputDataDir指向的目录
        _tables = new Tables(file => JSON.Parse(File.ReadAllText($"{gameConfDir}/{file}.json")));
        yield break;
    }

    public Dictionary<int, BattleBuffConfig> GetBattleBuffConfigMap()
    {
        return _tables.TbBattleBuffConfig.DataMap;
    }

    public BattleBuffConfig GetBattleBuffConfig(int buffID)
    {
        return _tables.TbBattleBuffConfig.DataMap.GetValueOrDefault(buffID, null);
    }

    public Dictionary<int, BattleBuffRelationConfig> GetBattleBuffRelationConfigMap()
    {
        return _tables.TbBattleBuffRelationConfig.DataMap;
    }

    public BattleBuffRelationConfig GetBattleBuffRelationConfig(int buffID)
    {
        return _tables.TbBattleBuffRelationConfig.DataMap.GetValueOrDefault(buffID, null);
    }

    public Dictionary<int, BattleSkillConfig> GetBattleSkillConfigMap()
    {
        return _tables.TbBattleSkillConfig.DataMap;
    }

    public BattleSkillConfig GetBattleSkillConfig(int skillID)
    {
        return _tables.TbBattleSkillConfig.DataMap.GetValueOrDefault(skillID, null);
    }

    public Dictionary<int, HeartMethodConfig> GetHeartMethodConfigMap()
    {
        return _tables.TbHeartMethodConfig.DataMap;
    }

    public HeartMethodConfig GetHeartMethodConfig(int heartMethodID)
    {
        return _tables.TbHeartMethodConfig.DataMap.GetValueOrDefault(heartMethodID, null);
    }
    
    public Dictionary<int, TreasureConfig> GetTreasureConfigMap()
    {
        return _tables.TbTreasureConfig.DataMap;
    }

    public TreasureConfig GetTreasureConfig(int treasureID)
    {
        return _tables.TbTreasureConfig.DataMap.GetValueOrDefault(treasureID, null);
    }

    public Dictionary<int, BattleMomentConfig> GetBattleMomentConfigMap()
    {
        return  _tables.TbBattleMomentConfig.DataMap;
    }

    public BattleMomentConfig GetBattleMomentConfig(int battleMomentID)
    {
        return  _tables.TbBattleMomentConfig.DataMap.GetValueOrDefault(battleMomentID, null);
    }

    public Dictionary<int, BattleMomentConditionConfig> GetBattleMomentConditionConfigMap()
    {
        return  _tables.TbBattleMomentConditionConfig.DataMap;
    }

    public BattleMomentConditionConfig GetBattleMomentConditionConfig(int battleMomentConditionID)
    {
        return  _tables.TbBattleMomentConditionConfig.DataMap.GetValueOrDefault(battleMomentConditionID, null);
    }

    public Dictionary<int, BattleMomentEffectConfig> GetBattleMomentEffectConfigMap()
    {
        return   _tables.TbBattleMomentEffectConfig.DataMap;
    }

    public BattleMomentEffectConfig GetBattleMomentEffectConfig(int battleMomentEffectID)
    {
        return _tables.TbBattleMomentEffectConfig.DataMap.GetValueOrDefault(battleMomentEffectID, null);
    }
    
    public Dictionary<string, TimeConfig> GetTimeConfigMap()
    {
        return _tables.TbTimeConfig.DataMap;
    }

    public TimeConfig GetTimeConfig(int year, int month, int day)
    {
        var key = $"{year}{month}{day:02}";
        return _tables.TbTimeConfig.DataMap.GetValueOrDefault(key, null);
    }
}
