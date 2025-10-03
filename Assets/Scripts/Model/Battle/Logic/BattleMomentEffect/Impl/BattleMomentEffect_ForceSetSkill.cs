using cfg;

public class BattleMomentEffect_ForceSetSkill : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                target.ForceSetSkill(GetNewSkillID());
            }
        }
    }

    private int GetNewSkillID()
    {
        return 2;
    }
}