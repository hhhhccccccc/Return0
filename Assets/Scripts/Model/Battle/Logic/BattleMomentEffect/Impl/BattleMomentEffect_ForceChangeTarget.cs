using cfg;

public class BattleMomentEffect_ForceChangeTarget : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                target.ForceChangeTarget(GetNewTargetID());
            }
        }
    }

    private int GetNewTargetID()
    {
        return 3;
    }
}