using cfg;

public class BattleBuff40291 : BattleBuffPotion
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.BreakInt)
        {
            return GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr;
        }

        return 0;
    }
}
