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
    public override void Handle(BattleRoundEndEventModel model)
    {
        BattleManager.RoundEnd();
        BattleLogicBehaviourManager.RoundEnd();
        BattleLogicStateManager.RoundEnd();
        BattleRenderManager.RoundEnd();
        //GameInputManager.Instance.SetBattleInputValid(true);
    }
}
