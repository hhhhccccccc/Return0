using cfg;

public class BattleMomentEffect_SetProperty : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var unitParamID = Config.ParamList[0];
        var target = GetUnitByParamID(unitParamID);
        if (target != null)
        {
            var propertyType = Config.ParamList[1].ToInt();
            var propertyValue = Config.ParamList[2];
            var source = BattleSource.None;
            if (Config.ParamList.Count > 3)
            {
                source = (BattleSource)Config.ParamList[3].ToInt();
            }
       
            target.SetProperty((BattlePropertyType)propertyType, propertyValue, source);
        }
    }
}