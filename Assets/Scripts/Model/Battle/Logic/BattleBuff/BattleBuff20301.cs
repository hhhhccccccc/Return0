using cfg;

public class BattleBuff20301 : BattleBuffBase
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.GangQiRedInt)
        {
            return LayerCount * Config.ParamEx[0].ToInt();
        }

        return 0;
    }
}
