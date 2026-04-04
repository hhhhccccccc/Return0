using cfg;

//目标下回合刚炁自然恢复不会低于35，自身下回合玄炁自然恢复不会低于35
public class BattleBuff74073 : BattleBuffBase
{
    public override void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        if ((pType == BattlePropertyType.GangQi || pType == BattlePropertyType.XuanQi) &&
            source == BattleSource.Natural)
        {
            if (value < GetConfigParamFloat(0))
            {
                value = GetConfigParamFloat(0);
            }
        }
    }
}   
