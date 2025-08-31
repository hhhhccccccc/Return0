using cfg;

public class BattleMomentEffect_SetAccumulateDamage : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        { 
            target.SetAccumulateDamage();
        }
    }
}