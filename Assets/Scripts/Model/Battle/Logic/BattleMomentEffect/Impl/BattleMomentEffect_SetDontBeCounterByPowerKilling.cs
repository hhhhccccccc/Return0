using cfg;

public class BattleMomentEffect_SetDontBeCounterByPowerKilling : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var state = Config.ParamList[1].ToInt() == 1;
            foreach (var target in targetList)
            {
                target.SetDontBeCounterByPowerKilling(state ? 1 : -1);
            }
        }
    }
}