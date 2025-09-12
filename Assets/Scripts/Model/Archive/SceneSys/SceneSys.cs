using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class SceneSys : SingleArchiveModel
{
    [Inject] private ConfigManager ConfigManager { get; set; }
    
    /// <summary>
    /// 州（世界）
    /// </summary>
    public int WorldID;
    /// <summary>
    /// 县（地图）
    /// </summary>
    public int MapID;
    /// <summary>
    /// 地区
    /// </summary>
    public int ZoneID;
    /// <summary>
    /// 场景
    /// </summary>
    public int SceneID;
    
    public override void Init()
    {
        base.Init();
    }

    public void EnterZone(int zoneID)
    {
        if (ZoneID == zoneID)
        {
            return;
        }

        var model = GetClass<ZoneChangedEventModel>();
        model.OldZoneID = ZoneID;
        model.NewZoneID = zoneID;
        ZoneID = zoneID;
        Dispatch(model);
        RecycleClass(model);
        
        var zoneConfig = ConfigManager.GetZoneConfig(zoneID);
        EnterScene(zoneConfig.EntrySceneID);
    }

    public void EnterScene(int sceneID, bool force = false)
    {
        if (SceneID == sceneID && !force)
        {
            return;
        }
        
        var model = GetClass<SceneChangedEventModel>();
        model.OldSceneID = SceneID;
        model.NewSceneID = sceneID;
        SceneID = sceneID;
        Dispatch(model);
        RecycleClass(model);
    }

    #region 表处理

    /// <summary>
    /// 获取某个州的所有县Config
    /// </summary>
    private List<MapConfig> TempMapConfigs = new();
    public List<MapConfig> GetMapConfigsByWorldID(int worldID)
    {
        TempMapConfigs.Clear();
        foreach (var kv in ConfigManager.GetMapConfigMap())
        {
            if (kv.Value.WorldID == worldID)
            {
                TempMapConfigs.Add(kv.Value);
            }
        }

        return TempMapConfigs;
    }
    
    /// <summary>
    /// 获取某个县的所有地区Config
    /// </summary>
    private List<ZoneConfig> TempZoneConfigs = new();
    public List<ZoneConfig> GetZoneConfigsByMapID(int mapID)
    {
        TempZoneConfigs.Clear();
        foreach (var kv in ConfigManager.GetZoneConfigMap())
        {
            if (kv.Value.MapID == mapID)
            {
                TempZoneConfigs.Add(kv.Value);
            }
        }

        return TempZoneConfigs;
    }
    
    /// <summary>
    /// 获取某个地区的所有场景Config
    /// </summary>
    private List<SceneConfig> TempSceneConfigs = new();
    public List<SceneConfig> GetSceneConfigsByMapID(int zoneID)
    {
        TempSceneConfigs.Clear();
        foreach (var kv in ConfigManager.GetSceneConfigMap())
        {
            if (kv.Value.ZoneID == zoneID)
            {
                TempSceneConfigs.Add(kv.Value);
            }
        }

        return TempSceneConfigs;
    }

    /// <summary>
    /// 获取当前Scene下可刷新的路人
    /// </summary>
    private List<PasserbyConfig> TempPasserbyConfigs = new();

    public List<PasserbyConfig> GetCheckConditionPasserbyConfigs()
    {
        bool CheckScene(PasserbyConfig config)
        {
            return config.SceneLocation.Any(location => location.SceneID == SceneID);
        }
        
        TempPasserbyConfigs.Clear();
        foreach (var kv in ConfigManager.GetPasserbyConfigMap())
        {
            var config = kv.Value;
            if (CheckScene(config))
            {
                TempPasserbyConfigs.Add(config);
            }
        }

        return TempPasserbyConfigs;
    }
    
    /// <summary>
    /// 随机count个可刷新的路人
    /// </summary>
    /// <param name="count"></param>
    /// <returns></returns>
    public List<PasserbyConfig> GetRandomPasserbyInCurrentScene(int count)
    {
        var configs = GetCheckConditionPasserbyConfigs();
        return Util.GetRandomNoSame(configs, configs.Select(config => config.Weight).ToList(), count);
    }
    
    #endregion
}
