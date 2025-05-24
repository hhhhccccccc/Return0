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
        
        var mapObj = PoolManager.GetGameObject("Assets/Prefab/Battle/Theme/BattleTheme.prefab");

        var themeManager = mapObj.GetComponent<ThemeManager>();

        DiContainer.Unbind<ThemeManager>();
        DiContainer.Bind<ThemeManager>().FromInstance(themeManager);
        
        UIManager.ShowUI("UIBattle");
        
        MessageManager.Dispatch<BattleStartEventModel>(null);
    }
}
