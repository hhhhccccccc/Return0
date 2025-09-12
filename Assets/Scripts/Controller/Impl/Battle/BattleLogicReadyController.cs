using UnityEngine;
using Zenject;

public class BattleLogicReadyContoller : ControllerBase<BattleLogicReadyEventModel>
{
    [Inject] private DiContainer DiContainer;
    [Inject] private ILogManager LogManager;
    [Inject] private UIManager UIManager;
    [Inject] private IPoolManager PoolManager;
    [Inject] private IMessageManager MessageManager;
    public override void Handle(BattleLogicReadyEventModel model)
    {
        LogManager.Debug("战斗逻辑层加载完毕");
        var managerObj = PoolManager.GetGameObject("Assets/GameResource/Prefab/Battle/BattleRenderManager.prefab");
        var battleRenderManager = managerObj.GetComponent<BattleRenderManager>();
        DiContainer.Unbind<BattleRenderManager>();
        DiContainer.Bind<BattleRenderManager>().FromInstance(battleRenderManager);
        battleRenderManager.AfterBind();
        UIManager.ShowUI<UIBattlePanel>();
        MessageManager.DispatchMsg<BattleStartEventModel>(null);
    }
}
