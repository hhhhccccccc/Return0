using cfg;

public class BattleMomentEffect_SetAccumulateDamage : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            foreach (var target in targetList)
            {
                target.SetAccumulateDamage();
            }
        }
    }
}