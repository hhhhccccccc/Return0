using UnityEngine;
using Zenject;

public class BattleStartController : ControllerBase<BattleStartEventModel>
{
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleDataManager BattleDataManager;
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    public override void Handle(BattleStartEventModel model)
    {
        BattleLogicBehaviourManager.BattleStart();
        BattleLogicStateManager.BattleStart();
        BattleManager.BattleStart();
    }
}
