using UnityEngine;
using Zenject;

public class GameStartController : ControllerBase<GameStartEventModel>
{
    [Inject] private SceneSys SceneSys { get; set; }
    public override void Handle(GameStartEventModel model)
    {
        LogManager.D("游戏开始");
        //绑定场景管理器
        var managerObj = PoolManager.GetGameObject("Assets/GameResource/Prefab/Scene/SceneManager.prefab", ViewManager.Root);
        var sceneManager = managerObj.GetComponent<SceneManager>();
        DiContainer.Unbind<SceneManager>();
        DiContainer.Bind<SceneManager>().FromInstance(sceneManager);
        
        //SceneSys.EnterScene(1, true);
        //SceneSys.EnterScene(2);
        //SceneSys.EnterScene(3);
    }
}
