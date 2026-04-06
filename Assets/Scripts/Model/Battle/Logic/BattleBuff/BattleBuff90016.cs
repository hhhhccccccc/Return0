using cfg;

//todo 下回合不会自然恢复炁
public class BattleBuff90016 : BattleBuffBase
{
    protected override void OnBuffStart()
    {
        Subject.AddNotRecoverGangQiNatural(1);
        Subject.AddNotRecoverXuanQiNatural(1);
    }

    protected override void OnBuffRemove()
    {
        Subject.AddNotRecoverGangQiNatural(-1);
        Subject.AddNotRecoverXuanQiNatural(-1);
    }
}
