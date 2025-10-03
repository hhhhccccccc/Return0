using cfg;

public class BattleMomentEffect_SetBeDamageInSkillAction : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                var skill = target.GetSkill();
                if (skill != null)
                {
                    skill.SetBeDamageInSkillAction();
                }
            }
        }
    }
}