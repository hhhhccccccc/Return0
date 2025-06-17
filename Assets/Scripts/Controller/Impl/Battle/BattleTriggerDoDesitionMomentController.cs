using System.Linq;
using UnityEngine;
using Zenject;

public class BattleTriggerDoDesitionMomentController : ControllerBase<BattleTriggerDoDesitionMomentEventModel>
{
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    public override void Handle(BattleTriggerDoDesitionMomentEventModel model)
    {
        foreach (var entityID in model.DoDesitionUnitList)
        {
            var unit = BattleManager.GetUnit(entityID);
            foreach (var moment in unit.GetBattleMoment())
            {
                moment.DoDesitionAction();
            }
        }
    }
}
