using cfg;

public class BattleMomentEffect_AddActionTimes : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            var times = Config.ParamList[1].ToInt();
            target.AddActionTimes(times);
        }
    }
}