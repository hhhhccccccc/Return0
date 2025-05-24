using UnityEngine;
using Zenject;

public class BattleStartController : ControllerBase<BattleStartEventModel>
{
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleDataManager BattleDataManager;
    [Inject] private BattleInputManager BattleInputManager;
    public override void Handle(BattleStartEventModel model)
    {
        BattleInputManager.BattleStart();
        BattleManager.BattleStart();
    }
}
