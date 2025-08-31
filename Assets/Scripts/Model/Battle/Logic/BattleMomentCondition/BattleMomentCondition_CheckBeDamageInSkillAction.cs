public class BattleMomentCondition_CheckBeDamageInSkillAction : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            return target.GetBeDamageInSkillAction();
        }

        return false;
    }
}