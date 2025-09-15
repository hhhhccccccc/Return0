using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckMutualGoal : BattleMomentCondition
{
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager;
    
    protected override bool OnCondition()
    {
        var subjectBehaviour = BattleLogicBehaviourManager.BattleBehaviourRes.TryGetValue(Subject.EntityID);
        var targetBehaviour = BattleLogicBehaviourManager.BattleBehaviourRes.TryGetValue(Target.EntityID);
        return subjectBehaviour.TargetID == Target.EntityID && targetBehaviour.TargetID == Subject.EntityID;
    }
}