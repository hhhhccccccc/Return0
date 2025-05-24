using UnityEngine;
using Zenject;

public class BattleRoundStartController : ControllerBase<BattleRoundStartEventModel>
{
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleDataManager BattleDataManager;
    public override void Handle(BattleRoundStartEventModel model)
    {
        BattleManager.RoundStart();
        InputManager.Instance.SetBattleInputValid(true);
    }
}
