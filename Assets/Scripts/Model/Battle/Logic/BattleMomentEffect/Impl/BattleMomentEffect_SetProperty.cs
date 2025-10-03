using cfg;

public class BattleMomentEffect_SetProperty : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var targetList = GetUnitByParamID(Config.ParamList[0]);
        if (targetList.Count > 0)
        {
            var propertyType = Config.ParamList[1].ToInt();
            var propertyValue = Config.ParamList[2];
            var source = BattleSource.None;
            if (Config.ParamList.Count > 3)
            {
                source = (BattleSource)Config.ParamList[3].ToInt();
            }

            foreach (var target in targetList)
            {
                target.SetProperty((BattlePropertyType)propertyType, propertyValue, source);
            }
        }
    }
}