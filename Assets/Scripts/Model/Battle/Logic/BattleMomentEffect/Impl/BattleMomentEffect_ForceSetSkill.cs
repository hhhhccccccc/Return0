using cfg;

public class BattleMomentEffect_ForceSetSkill : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        { 
            target.ForceSetSkill(GetNewSkillID());
        }
    }

    private int GetNewSkillID()
    {
        return 2;
    }
}