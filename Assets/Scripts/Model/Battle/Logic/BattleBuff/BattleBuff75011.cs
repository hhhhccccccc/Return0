using cfg;

public class BattleBuff75011 : BattleBuffBase
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.XuanQiRecNatural)
        {
            return 3;
        }
        
        return 0;
    }
}
