using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckSkillLastClashState : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null && ParamModel is DamageParamModel model)
        {
            var skillID = Config.ParamList[1].ToInt();
            var checkState = Config.ParamList[2].ToInt() == 1;
            return target.UseSkillDataManager.CheckSkillLastClashState(skillID, checkState);
        }
        return false;
    }
}