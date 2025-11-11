public class BattleBuff72054 : BattleBuffBase
{
    public override (float, float) ChangeResourceCost(float gangQiCost, float xuanQiCost)
    {
        return (0, gangQiCost + xuanQiCost);
    }
}
