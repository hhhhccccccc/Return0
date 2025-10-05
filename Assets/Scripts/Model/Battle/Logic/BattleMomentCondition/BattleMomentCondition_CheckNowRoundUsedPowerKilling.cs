using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckNowRoundUsedPowerKilling : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            return target.UseSkillDataManager.CheckNowRoundUsedPowerKilling();
        }

        return false;
    }
}