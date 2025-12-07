using cfg;

public class BattleBuff40161 : BattleBuffPotion
{
    protected override void OnRoundEnd()
    {
        Subject.ChangeKey(BattleKeyType.KeyLeft, Config.ParamEx[0].ToInt(), ChangeKeyReason.Item);
    }
}
