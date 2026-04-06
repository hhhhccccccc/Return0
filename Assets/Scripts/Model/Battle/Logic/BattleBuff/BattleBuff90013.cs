using cfg;

//todo 不影响状态的存续且延迟1息
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
