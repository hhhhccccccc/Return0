using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleMomentEffect_ChangeNearlyBeActionTargetToTeamOther : BattleMomentEffect
{
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }
    protected override void OnEffect()
    { 
        BattleUnit unit = null;
        BattleBehaviour b = null;
        int minActionWheel = 99999;
        
        var behaviourList = BattleLogicBehaviourManager.BattleBehaviourRes.GetListValue();
        foreach (var behaviour in behaviourList)
        {
            if (behaviour.TargetID == Subject.EntityID)
            {
                var target = BattleManager.GetUnit(behaviour.SubjectID);
                if (target != null && target.ActionWheel < minActionWheel)
                {
                    unit = target;
                    minActionWheel = target.ActionWheel;
                    b = behaviour;
                }
            }
        }

        if (unit != null)
        {
            var subjectBehaviour = BattleLogicBehaviourManager.BattleBehaviourRes.TryGetValue(Subject.EntityID);
            var toTarget = BattleManager.GetUnit(subjectBehaviour.TargetID);
            var skillBase = unit.GetSkill();
            if (skillBase != null)
            {
                skillBase.SetTarget(toTarget);
                b.TargetID = toTarget.EntityID;
            }
        }
    }
}