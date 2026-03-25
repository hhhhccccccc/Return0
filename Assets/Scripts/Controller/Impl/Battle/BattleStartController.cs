using UnityEngine;
using Zenject;

public class BattleStartController : ControllerBase<BattleStartEventModel>
{
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleDataManager BattleDataManager;
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    [Inject] private BattleAIManager BattleAIManager;
    public override void Handle(BattleStartEventModel model)
    {
        BattleAIManager.BattleStart();
        BattleManager.BattleStart();
        BattleLogicBehaviourManager.BattleStart();
        BattleLogicStateManager.BattleStart();
    }
}
