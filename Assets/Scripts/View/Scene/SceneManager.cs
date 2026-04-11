using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SceneManager : View
{
    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private ConfigManager ConfigManager { get; set; }
    private Dictionary<int, Scene> SceneDic = new();

    protected override void RegisterEvent()
    {
        base.RegisterEvent();
        Register<SceneChangedEventModel>(OnSceneChanged);
    }

    private void HideAllScene()
    {
        foreach (var kv in SceneDic)
        {
            kv.Value.OnSceneHide();
        }
    }

    private void OnSceneChanged(SceneChangedEventModel model)
    {
        var oldSceneID = model.OldSceneID;
        if (SceneDic.TryGetValue(oldSceneID, out var scene))
        {
            scene.OnSceneHide();
        }
        
        var newSceneID = model.NewSceneID;
        if (!SceneDic.TryGetValue(newSceneID, out scene))
        {
            var sceneConfig = ConfigManager.GetSceneConfig(newSceneID);
            var sceneObj = PoolManager.GetGameObject($"{GameConst.View.SceneRoot}{sceneConfig.SceneResource}.prefab", ViewManager.Root);
            sceneObj.transform.SetParent(transform);
            sceneObj.transform.localScale = Vector3.one;
            scene = sceneObj.GetComponent<Scene>();
            scene.OnSceneCreate(newSceneID);
            SceneDic.Add(newSceneID, scene);
        }
        
        scene.OnSceneShow();
    }
}
