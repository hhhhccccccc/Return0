using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckSkillKillingStyle : BattleMomentCondition
{
    [Inject] private BattleUtil BattleUtil;
    protected override bool OnCondition()
    {
        var relation = Config.ParamList[1].ToInt();
        if (SkillID != 0)
        {
            if (relation == 1)
            {
                return BattleUtil.SkillIsKillingStyle(SkillID);
            }
            else
            {
                return !BattleUtil.SkillIsKillingStyle(SkillID);
            }
        }

        var target = GetUnitByParamID(Config.ParamList[0]);
        var skillID = target.GetSkillID();
        if (skillID != 0)
        {
            if (relation == 1)
            {
                return BattleUtil.SkillIsKillingStyle(skillID);
            }
            else
            {
                return !BattleUtil.SkillIsKillingStyle(skillID);
            }
        }

        return false;
    }
}