using cfg;

public class BattleBuff30301 : BattleBuffBase
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.Power || propertyType == BattlePropertyType.Tech)
        {
            return GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr;
        }

        return 0;
    }
}
