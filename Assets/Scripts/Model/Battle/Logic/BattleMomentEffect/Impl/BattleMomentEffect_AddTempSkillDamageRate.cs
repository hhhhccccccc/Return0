using cfg;

public class BattleMomentEffect_AddTempSkillDamageRate : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            var addSkillDamageValue = Config.ParamList[1];
            target.AddTempSkillDamageValue(addSkillDamageValue);
        }
    }
}