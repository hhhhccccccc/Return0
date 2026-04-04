using cfg;

//下回合刚炁自然恢复不会低于35
public class BattleBuff740731 : BattleBuffBase
{
    public override void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        if ((pType == BattlePropertyType.GangQi) &&
            source == BattleSource.Natural)
        {
            if (value < GetConfigParamFloat(0))
            {
                value = GetConfigParamFloat(0);
            }
        }
    }
}   
