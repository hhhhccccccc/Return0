using cfg;

public class BattleMomentEffect_ChangeActionWheel : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            var changeActionWheelValue = Config.ParamList[1].ToInt();
            target.ChangeActionWheel(changeActionWheelValue);
        }
    }
}