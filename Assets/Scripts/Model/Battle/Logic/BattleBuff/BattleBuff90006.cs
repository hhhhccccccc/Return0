using cfg;

public class BattleBuff90006 : BattleBuffBase
{
    protected override void OnDoDesitionAction(bool isPreDesition)
    {
        DoAddActionTimes(Subject, 1);
    }
}
