using cfg;

//下回合玄炁自然恢复不会低于35
public class BattleBuff740732 : BattleBuffBase
{
    public override void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        if ((pType == BattlePropertyType.XuanQi) &&
            source == BattleSource.Natural)
        {
            if (value < GetConfigParamFloat(0))
            {
                value = GetConfigParamFloat(0);
            }
        }
    }
}   
