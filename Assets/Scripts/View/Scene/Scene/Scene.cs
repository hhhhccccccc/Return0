using System.Collections.Generic;
using cfg;
using UnityEngine;
using Zenject;

public class Scene : View
{
    #region 节点

    [AutoFind] private Transform PasserbyNode { get; set; }

    #endregion
    [Inject] private SceneSys SceneSys { get; set; }
    [Inject] private ConfigManager ConfigManager { get; set; }
    [Inject] private IPoolManager PoolManager { get; set; }
    private int SceneID { get; set; }
    private SceneConfig SceneConfig { get; set; }

    private List<PasserbyConfig> PasserbyConfigList = new();

    private List<Passerby> PasserbyList = new();
    public void OnSceneCreate(int sceneID)
    {
        SceneID = sceneID;
        SceneConfig = ConfigManager.GetSceneConfig(sceneID);
    }
    
    public virtual void OnSceneShow()
    {
        GeneratePasserby();
    }

    public virtual void OnSceneHide()
    {
        SetActive(false);
    }

    private void GeneratePasserby()
    {
        var passerbyConfigs = SceneSys.GetRandomPasserbyInCurrentScene(4);
        PasserbyConfigList = passerbyConfigs.Clone();
        PasserbyList.Clear();
        foreach (var config in PasserbyConfigList)
        {
            Debug(config.Resource);
            var passerbyObj = PoolManager.GetGameObject($"{GameConst.View.PasserbyRoot}{config.Resource}.prefab");
            var passerby = passerbyObj.GetComponent<Passerby>();
            passerby.Init(config.ID, SceneID);
            passerby.transform.SetParent(PasserbyNode);
            PasserbyList.Add(passerby);
        }
    }
}
