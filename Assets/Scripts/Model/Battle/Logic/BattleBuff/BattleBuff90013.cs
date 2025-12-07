using cfg;

public class BattleBuff90013 : BattleBuffBase
{
    protected override void OnBuffStart()
    {
        Subject.AddStatusPersists(1);
    }

    protected override void OnBuffRemove()
    {
        Subject.AddStatusPersists(-1);
    }
}
