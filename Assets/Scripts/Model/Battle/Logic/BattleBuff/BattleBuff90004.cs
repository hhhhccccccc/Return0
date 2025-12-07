using cfg;

public class BattleBuff90004 : BattleBuffBase
{
    protected override void OnRoundStart()
    {
        base.OnRoundStart();
        Subject.RemoveAllKey();
        Subject.ClearBuff(BuffID);
    }
}
