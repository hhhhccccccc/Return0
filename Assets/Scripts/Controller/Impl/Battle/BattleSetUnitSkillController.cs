using System.Linq;
using UnityEngine;
using Zenject;

public class BattleSetUnitSkillController : ControllerBase<BattleSetUnitSkillEventModel>
{
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    public override void Handle(BattleSetUnitSkillEventModel model)
    {
        foreach (var entityID in model.SetSkillUnitList)
        {
            var behaviour = BattleLogicBehaviourManager.GetBattleBehaviour(entityID);
            var subject = BattleManager.GetUnit(behaviour.SubjectID);
            var target = BattleManager.GetUnit(behaviour.TargetID);
            subject.SetSkill(behaviour.SkillID, target);
        }
    }
}
