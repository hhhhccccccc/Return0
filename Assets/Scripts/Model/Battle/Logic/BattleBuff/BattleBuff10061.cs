using cfg;

public class BattleBuff10061 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.XuanQiRecInt)
        {
            return LayerCount * Config.ParamEx[0];
        }

        return 0;
    }
}
