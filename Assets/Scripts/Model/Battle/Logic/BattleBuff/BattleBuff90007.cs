using cfg;

public class BattleBuff90007 : BattleBuffBase
{
    protected override float OnGetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.RecoverGangQiBySkillOffset ||
            propertyType == BattlePropertyType.RecoverXuanQiBySkillOffset)
        {
            return GetConfigParamFloat(0);
        }

        return 0;
    }
}
