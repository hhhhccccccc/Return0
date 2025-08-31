using cfg;

public class BattleMomentEffect_SetBeDamageInSkillAction : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        { 
            target.SetBeDamageInSkillAction();
        }
    }
}