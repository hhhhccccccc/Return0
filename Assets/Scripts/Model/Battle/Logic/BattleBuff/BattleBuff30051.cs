using cfg;

public class BattleBuff30051 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.SpeedInt)
        {
            var count = LayerCount / Config.ParamEx[0].ToInt();
            return count * Config.ParamEx[1];
        }

        if (propertyType == BattlePropertyType.PowerInt)
        {
            var count = LayerCount / Config.ParamEx[0].ToInt();
            return count * Config.ParamEx[2];
        }

        if (propertyType == BattlePropertyType.TechInt)
        {
            var count = LayerCount / Config.ParamEx[0].ToInt();
            return count * Config.ParamEx[3];
        }

        return 0;
    }
}
