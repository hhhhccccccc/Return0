using cfg;

public class BattleMomentEffect_SetDontBeCounterByPowerKilling : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            var state = Config.ParamList[1].ToInt() == 1;
            target.SetDontBeCounterByPowerKilling(state);
        }
    }
}