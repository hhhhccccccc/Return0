using UnityEngine;
using Zenject;

public class GameStartController : ControllerBase<GameStartEventModel>
{
    [Inject] private ILogManager LogManager;
    [Inject] private IPoolManager PoolManager;
    [Inject] private SceneSys SceneSys;
    public override void Handle(GameStartEventModel model)
    {
        LogManager.Debug("游戏开始");
        //绑定场景管理器
        var managerObj = PoolManager.GetGameObject("Assets/GameResource/Prefab/Scene/SceneManager.prefab");
        var sceneManager = managerObj.GetComponent<SceneManager>();
        DiContainer.Unbind<SceneManager>();
        DiContainer.Bind<SceneManager>().FromInstance(sceneManager);
        
        //SceneSys.EnterScene(1, true);
        //SceneSys.EnterScene(2);
        //SceneSys.EnterScene(3);
    }
}
