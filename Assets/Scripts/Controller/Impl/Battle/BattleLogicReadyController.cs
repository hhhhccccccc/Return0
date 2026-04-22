using UnityEngine;
using Zenject;

public class BattleLogicReadyContoller : ControllerBase<BattleLogicReadyEventModel>
{
    public override void Handle(BattleLogicReadyEventModel model)
    {
        LogManager.D("战斗逻辑层加载完毕");
        var managerObj = PoolManager.GetGameObject("Assets/GameResource/Prefab/BattleRenderManager.prefab", ViewManager.Root);
        var battleRenderManager = managerObj.GetComponent<BattleRenderManager>();
        DiContainer.Unbind<BattleRenderManager>();
        DiContainer.Bind<BattleRenderManager>().FromInstance(battleRenderManager);
        battleRenderManager.AfterBind();
        UIManager.ShowUI<UIBattlePanel>();
        MessageManager.DispatchMsg<BattleStartEventModel>(null);
    }
}
