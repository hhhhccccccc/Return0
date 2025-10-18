using cfg;

public class BattleBuff72053 : BattleBuffBase
{
    public override (float, float) ChangeResourceCost(float gangQiCost, float xuanQiCost)
    {
        return (gangQiCost + xuanQiCost, 0);
    }
}
