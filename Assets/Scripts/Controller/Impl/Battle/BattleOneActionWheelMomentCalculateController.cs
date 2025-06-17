using System.Linq;
using UnityEngine;
using Zenject;

public class BattleOneActionWheelMomentCalculateController : ControllerBase<BattleOneActionWheelMomentCalculateEventModel>
{
    [Inject] private BattleManager BattleManager;
    [Inject] private BattleDataManager BattleDataManager;
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    public override void Handle(BattleOneActionWheelMomentCalculateEventModel model)
    {
        BattleLogicStateManager.SetBattleState(BattleState.ActionWheelMomentCalculate);
        var actionUnits = model.ActionWheelUnit.Select(entityID => BattleManager.GetUnit(entityID)).ToList();//当前息行动的角色
        var actionBehaviours = BattleLogicBehaviourManager.BattleBehaviourRes.GetListValue()
            .Where(behaviour => actionUnits.Any(unit => unit.EntityID == behaviour.SubjectID)).ToList();//行动角色的指令

        foreach (var behaviour in actionBehaviours)
        {
            var subject = BattleManager.GetUnit(behaviour.SubjectID);
            foreach (var moment in subject.GetBattleMoment())
            {
                moment.StartActionWheel();
            }  

            var target = BattleManager.GetUnit(behaviour.TargetID);
            var isTeam = subject.Bf.Uid == target.Bf.Uid;
            var skillID = behaviour.SkillID;
            foreach (var moment in target.GetBattleMoment())
            {
                moment.AsTargetAction(isTeam, skillID);
            }
        }

        BattleLogicStateManager.OneActionWheelLogicCalculate();
    }
}
