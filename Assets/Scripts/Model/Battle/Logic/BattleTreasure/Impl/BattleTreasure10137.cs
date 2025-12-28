public class BattleTreasure10137 : BattleTreasureBase
{
    protected override void OnBattleStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10011, Subject, GetParamInt(0));
    }

    protected override void OnRoundStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10011, Subject, GetParamInt(1));
    }
}