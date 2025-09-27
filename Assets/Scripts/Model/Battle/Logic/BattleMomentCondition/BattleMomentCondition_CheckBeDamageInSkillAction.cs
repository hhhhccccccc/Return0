public class BattleMomentCondition_CheckBeDamageInSkillAction : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var skill = target.GetSkill();
            if (skill != null)
            {
                return skill.GetBeDamageInSkillAction();
            }
        }

        return false;
    }
}