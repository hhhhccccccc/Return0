using cfg;

public class BattleBuff10071 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.PowerPct)
        {
            return LayerCount * Config.ParamEx[0];
        }

        return 0;
    }
}
