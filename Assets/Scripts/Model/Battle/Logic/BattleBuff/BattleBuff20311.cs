using cfg;

public class BattleBuff20311 : BattleBuffBase
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.XuanQiRedInt)
        {
            return LayerCount * Config.ParamEx[0].ToInt();
        }

        return 0;
    }
}
