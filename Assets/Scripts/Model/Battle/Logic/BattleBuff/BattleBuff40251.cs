using cfg;

public class BattleBuff40251 : BattleBuffPotion
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.CleverInt)
        {
            return GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr;
        }

        return 0;
    }
}
