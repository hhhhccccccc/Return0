using cfg;

public class BattleBuff90014 : BattleBuffBase
{
    protected override void OnRoundStart()
    {
        DoAddActionTimes(Subject, 1);
    }
}
