using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckSkillTriggerMoment : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var skillBase = target.GetSkill();
            if (skillBase != null)
            {
                var momentType = Config.ParamList[1].ToInt();
                return skillBase.CheckTriggerMoment((BattleMomentType)momentType);
            }
        }

        return false;
    }
}