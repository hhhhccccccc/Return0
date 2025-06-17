using UnityEngine;
using Zenject;

public class BattleRoundStartController : ControllerBase<BattleRoundStartEventModel>
{
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleDataManager BattleDataManager;
    [Inject] private BattleRenderManager BattleRenderManager;
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    [Inject] private InputManager InputManager;
    public override void Handle(BattleRoundStartEventModel model)
    {
        BattleManager.RoundStart();//回合开始
        BattleLogicBehaviourManager.RoundStart();
        BattleLogicStateManager.RoundStart();
        BattleRenderManager.RoundStart();
        InputManager.SetBattleInputValid(true);
    }
}
