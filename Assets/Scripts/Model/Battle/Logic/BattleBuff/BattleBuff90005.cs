using cfg;

public class BattleBuff90005 : BattleBuffBase
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.XuanQiRedInt)
        {
            return GetConfigParamFloat(0);
        }

        return 0;
    }
}
