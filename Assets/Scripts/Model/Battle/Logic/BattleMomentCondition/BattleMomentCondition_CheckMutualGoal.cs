using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckMutualGoal : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var subjectSkill = Subject.GetSkill();
        var targetSkill = Target.GetSkill();
        if (subjectSkill != null && targetSkill != null)
        {
            return subjectSkill.Target == Target && targetSkill.Target == Subject;
        }

        return false;
    }
}