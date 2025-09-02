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
    
    public Dictionary<string, DateConfig> GetDateConfigMap()
    {
        return _tables.TbDateConfig.DataMap;
    }

    public DateConfig GetDateConfig(int year, int month, int day)
    {
        return _tables.TbDateConfig.DataMap.GetValueOrDefault($"{year}{month}{day:02}", null);
    }
    
    public Dictionary<int, DateTypeConfig> GetDateTypeConfigMap()
    {
        return _tables.TbDateTypeConfig.DataMap;
    }

    public DateTypeConfig GetDateTypeConfig(int dateTypeID)
    {
        return _tables.TbDateTypeConfig.DataMap.GetValueOrDefault(dateTypeID, null);
    }
    
    public Dictionary<int, SeasonConfig> GetSeasonConfigMap()
    {
        return _tables.TbSeasonConfig.DataMap;
    }

    public SeasonConfig GetSeasonConfig(int seasonID)
    {
        return _tables.TbSeasonConfig.DataMap.GetValueOrDefault(seasonID, null);
    }
    
    public Dictionary<int, TimeCostConfig> GetTimeCostConfigMap()
    {
        return _tables.TbTimeCostConfig.DataMap;
    }

    public TimeCostConfig GetTimeCostConfig(int timeCostID)
    {
        return _tables.TbTimeCostConfig.DataMap.GetValueOrDefault(timeCostID, null);
    }
    
    public Dictionary<int, ConditionConfig> GetConditionConfigMap()
    {
        return _tables.TbConditionConfig.DataMap;
    }

    public ConditionConfig GetConditionConfig(int conditionID)
    {
        return _tables.TbConditionConfig.DataMap.GetValueOrDefault(conditionID, null);
    }
    
    public Dictionary<int, SceneConfig> GetSceneConfigMap()
    {
        return _tables.TbSceneConfig.DataMap;
    }

    public SceneConfig GetSceneConfig(int sceneID)
    {
        return _tables.TbSceneConfig.DataMap.GetValueOrDefault(sceneID, null);
    }
    
    public Dictionary<int, SceneTypeConfig> GetSceneTypeConfigMap()
    {
        return _tables.TbSceneTypeConfig.DataMap;
    }

    public SceneTypeConfig GetSceneTypeConfig(int sceneTypeID)
    {
        return _tables.TbSceneTypeConfig.DataMap.GetValueOrDefault(sceneTypeID, null);
    }
    
    public Dictionary<int, SceneInteractionItemConfig> GetSceneInteractionItemConfigMap()
    {
        return _tables.TbSceneInteractionItemConfig.DataMap;
    }

    public SceneInteractionItemConfig GetSceneInteractionItemConfig(int itemID)
    {
        return _tables.TbSceneInteractionItemConfig.DataMap.GetValueOrDefault(itemID, null);
    }
    
    public Dictionary<int, MapConfig> GetMapConfigMap()
    {
        return _tables.TbMapConfig.DataMap;
    }

    public MapConfig GetMapConfig(int mapID)
    {
        return _tables.TbMapConfig.DataMap.GetValueOrDefault(mapID, null);
    }
    
    public Dictionary<int, ScenePassageConfig> GetScenePassageConfigMap()
    {
        return _tables.TbScenePassageConfig.DataMap;
    }

    public ScenePassageConfig GetScenePassageConfig(int passageID)
    {
        return _tables.TbScenePassageConfig.DataMap.GetValueOrDefault(passageID, null);
    }
    
    public Dictionary<int, ZoneConfig> GetZoneConfigMap()
    {
        return _tables.TbZoneConfig.DataMap;
    }

    public ZoneConfig GetZoneConfig(int zoneID)
    {
        return _tables.TbZoneConfig.DataMap.GetValueOrDefault(zoneID, null);
    }
    
    public Dictionary<int, WeatherConfig> GetWeatherConfigMap()
    {
        return _tables.TbWeatherConfig.DataMap;
    }

    public WeatherConfig GetWeatherConfig(int weatherID)
    {
        return _tables.TbWeatherConfig.DataMap.GetValueOrDefault(weatherID, null);
    }
    
    public Dictionary<int, WeatherGroupConfig> GetWeatherGroupConfigMap()
    {
        return _tables.TbWeatherGroupConfig.DataMap;
    }

    public WeatherGroupConfig GetWeatherGroupConfig(int weatherGroupID)
    {
        return _tables.TbWeatherGroupConfig.DataMap.GetValueOrDefault(weatherGroupID, null);
    }
    
    public Dictionary<int, WeatherPoolConfig> GetWeatherPoolConfigMap()
    {
        return _tables.TbWeatherPoolConfig.DataMap;
    }

    public WeatherPoolConfig GetWeatherPoolConfig(int weatherPoolID)
    {
        return _tables.TbWeatherPoolConfig.DataMap.GetValueOrDefault(weatherPoolID, null);
    }
    
    public Dictionary<int, TravelEventConfig> GetTravelEventConfigMap()
    {
        return _tables.TbTravelEventConfig.DataMap;
    }

    public TravelEventConfig GetTravelEventConfig(int eventID)
    {
        return _tables.TbTravelEventConfig.DataMap.GetValueOrDefault(eventID, null);
    }
    
    public Dictionary<int, TravelEventConditionConfig> GetTravelEventConditionConfigMap()
    {
        return _tables.TbTravelEventConditionConfig.DataMap;
    }

    public TravelEventConditionConfig GetTravelEventConditionConfig(int eventConditionID)
    {
        return _tables.TbTravelEventConditionConfig.DataMap.GetValueOrDefault(eventConditionID, null);
    }
    
    public Dictionary<int, PasserbyConfig> GetPasserbyConfigMap()
    {
        return _tables.TbPasserbyConfig.DataMap;
    }

    public PasserbyConfig GetPasserbyConfig(int passerbyID)
    {
        return _tables.TbPasserbyConfig.DataMap.GetValueOrDefault(passerbyID, null);
    }
    
    public Dictionary<int, NarratorDialogueConfig> GetNarratorDialogueConfigMap()
    {
        return _tables.TbNarratorDialogueConfig.DataMap;
    }

    public NarratorDialogueConfig GetNarratorDialogueConfig(int dialogueID)
    {
        return _tables.TbNarratorDialogueConfig.DataMap.GetValueOrDefault(dialogueID, null);
    }
}
