using cfg;

public class BattleBuff90012 : BattleBuffBase
{
    protected override float OnGetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.GangQiRecInt)
        {
            return Config.ParamEx[0];
        }

        return 0;
    }
}
