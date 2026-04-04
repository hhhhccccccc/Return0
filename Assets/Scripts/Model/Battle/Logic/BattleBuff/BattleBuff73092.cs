using cfg;

//本次战斗刚炁的自然恢复不会低于25
public class BattleBuff73092 : BattleBuffBase
{
    public override void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        if (pType == BattlePropertyType.GangQi &&
            source == BattleSource.Natural)
        {
            if (value < GetConfigParamFloat(0))
            {
                value = GetConfigParamFloat(0);
            }
        }
    }
}   
