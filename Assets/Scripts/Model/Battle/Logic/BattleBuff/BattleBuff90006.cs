using cfg;

public class BattleBuff90006 : BattleBuffBase
{
    protected override void OnDoDesitionAction()
    {
        Subject.AddActionTimes(1);
    }
}
