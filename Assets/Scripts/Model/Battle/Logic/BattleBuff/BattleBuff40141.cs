using cfg;

public class BattleBuff40141 : BattleBuffPotion
{
    protected override void OnRoundEnd()
    {
        Subject.ChangeKey(BattleKeyType.KeyUp, Config.ParamEx[0].ToInt(), ChangeKeyReason.Item);
    }
}
