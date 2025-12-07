using cfg;

public class BattleBuff40131 : BattleBuffPotion
{
    protected override void OnRoundEnd()
    {
        Subject.AddRandomKey(Config.ParamEx[0].ToInt(), ChangeKeyReason.Item);
    }
}
