using cfg;

public class BattleBuff10071 : BattleBuffBase
{
    /// <summary>
    /// 每层使力增加10%
    /// </summary>
    /// <param name="propertyType"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.PowerPct)
        {
            return LayerCount * GetConfigParamFloat(0);
        }

        return 0;
    }
}
