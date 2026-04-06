using cfg;

public class BattleBuff30051 : BattleBuffBase
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.SpeedInt)
        {
            var count = LayerCount / GetConfigParamInt(0);
            return count * GetConfigParamFloat(1);
        }

        if (propertyType == BattlePropertyType.PowerInt)
        {
            var count = LayerCount / GetConfigParamInt(0);
            return count * GetConfigParamFloat(2);
        }

        if (propertyType == BattlePropertyType.TechInt)
        {
            var count = LayerCount / GetConfigParamInt(0);
            return count * GetConfigParamFloat(3);
        }

        return 0;
    }
}
