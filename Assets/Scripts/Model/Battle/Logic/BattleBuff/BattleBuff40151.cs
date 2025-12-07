using cfg;

public class BattleBuff40151 : BattleBuffPotion
{
    protected override void OnRoundEnd()
    {
        Subject.ChangeKey(BattleKeyType.KeyDown, Config.ParamEx[0].ToInt(), ChangeKeyReason.Item);
    }
}
