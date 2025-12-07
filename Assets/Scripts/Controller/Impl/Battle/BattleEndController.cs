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
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    public override void Handle(BattleEndEventModel model)
    {
        LogManager.D("[战斗结束]");
        BattleManager.Clear();
        BattleLogicBehaviourManager.Clear();
        BattleLogicStateManager.Clear();
    }
}
