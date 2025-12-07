using cfg;

public class BattleBuff90007 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.RecoverGangQiBySkillOffset ||
            propertyType == BattlePropertyType.RecoverXuanQiBySkillOffset)
        {
            return Config.ParamEx[0];
        }

        return 0;
    }
}
