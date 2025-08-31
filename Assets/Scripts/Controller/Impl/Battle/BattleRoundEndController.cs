using UnityEngine;
using Zenject;

/// <summary>
/// 回合结束
/// </summary>
public class BattleRoundEndController : ControllerBase<BattleRoundEndEventModel>
{
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleDataManager BattleDataManager;
    [Inject] private BattleRenderManager BattleRenderManager;
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    [Inject] private BattleRecordManager BattleRecordManager;
    [Inject] private BattleAIManager BattleAIManager;
    public override void Handle(BattleRoundEndEventModel model)
    {
        BattleManager.RoundEnd();
        BattleLogicBehaviourManager.RoundEnd();
        BattleLogicStateManager.RoundEnd();
        BattleRenderManager.RoundEnd();
        BattleRecordManager.RoundEnd();
        BattleAIManager.RoundEnd();
        //GameInputManager.Instance.SetBattleInputValid(true);
    }
}
