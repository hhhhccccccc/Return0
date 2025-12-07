using cfg;

public class BattleBuff90005 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.XuanQiRedInt)
        {
            return Config.ParamEx[0];
        }

        return 0;
    }
}
