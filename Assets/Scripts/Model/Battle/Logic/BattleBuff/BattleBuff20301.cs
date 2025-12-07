using cfg;

public class BattleBuff20301 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.GangQiRedInt)
        {
            return LayerCount * Config.ParamEx[0].ToInt();
        }

        return 0;
    }
}
