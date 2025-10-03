using cfg;

public class BattleMomentEffect_SetDontBeCounterByArtKilling : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList != null)
        {
            var state = Config.ParamList[1].ToInt() == 1;
            foreach (var target in targetList)
            {
                target.SetDontBeCounterByArtKilling(state ? 1 : -1);
            }
        }
    }
}