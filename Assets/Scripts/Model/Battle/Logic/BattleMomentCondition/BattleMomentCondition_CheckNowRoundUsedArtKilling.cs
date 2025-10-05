using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckNowRoundUsedArtKilling : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            return target.UseSkillDataManager.CheckNowRoundUsedArtKilling();
        }

        return false;
    }
}