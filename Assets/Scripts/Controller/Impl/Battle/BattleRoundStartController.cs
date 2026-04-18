using UnityEngine;
using Zenject;

public class BattleRoundStartController : ControllerBase<BattleRoundStartEventModel>
{
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleDataManager BattleDataManager;
    [Inject] private BattleRenderManager BattleRenderManager;
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    [Inject] private BattleRecordManager BattleRecordManager;
    [Inject] private BattleAIManager BattleAIManager;
    [Inject] private InputManager InputManager;
    [Inject] private UIManager UIManager;
    public override void Handle(BattleRoundStartEventModel model)
    {
        BattleManager.RoundStart();//回合开始
        BattleLogicBehaviourManager.RoundStart();
        BattleLogicStateManager.RoundStart();
        BattleRenderManager.RoundStart();
        BattleAIManager.RoundStart();
        InputManager.SetBattleInputValid(true);

        UIManager.ShowUI<UIBattleRoundStartPanel>();
    }
}
