using cfg;

public class BattleMomentEffect_AddActionTimes : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var times = Config.ParamList[1].ToInt();
            foreach (var target in targetList)
            {
                target.AddActionTimes(times);
            }
        }
    }
}