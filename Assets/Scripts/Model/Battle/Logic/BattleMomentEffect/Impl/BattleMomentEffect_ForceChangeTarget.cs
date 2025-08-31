using cfg;

public class BattleMomentEffect_ForceChangeTarget : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        { 
            target.ForceChangeTarget(GetNewTargetID());
        }
    }

    private int GetNewTargetID()
    {
        return 3;
    }
}