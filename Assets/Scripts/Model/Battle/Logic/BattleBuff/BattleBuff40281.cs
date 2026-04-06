using cfg;

public class BattleBuff40281 : BattleBuffPotion
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.TechInt)
        {
            return GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr;
        }

        return 0;
    }
}
