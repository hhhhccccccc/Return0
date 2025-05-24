using UnityEngine;
using Zenject;

public class BattleEndController : ControllerBase<BattleEndEventModel>
{
    [Inject] private DiContainer DiContainer;
    [Inject] private ILogManager LogManager;
    [Inject] private UIManager UIManager;
    [Inject] private IPoolManager PoolManager;
    [Inject] private IMessageManager MessageManager;
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleInputManager BattleInputManager;
    public override void Handle(BattleEndEventModel model)
    {
        LogManager.Debug("[战斗结束]");
        BattleManager.Clear();
        BattleInputManager.Clear();
    }
}
