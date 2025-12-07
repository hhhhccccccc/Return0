using cfg;

public class BattleBuff30301 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.Power || propertyType == BattlePropertyType.Tech)
        {
            return Config.ParamEx[0] + Config.ParamEx[1] * Subject.Gr;
        }

        return 0;
    }
}
