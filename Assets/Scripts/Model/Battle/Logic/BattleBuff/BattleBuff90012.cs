using cfg;

public class BattleBuff90012 : BattleBuffBase
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.GangQiRecInt)
        {
            return GetConfigParamFloat(0);
        }

        return 0;
    }
}
