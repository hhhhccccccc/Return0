using cfg;

public class BattleBuff40171 : BattleBuffPotion
{
    protected override void OnRoundEnd()
    {
        Subject.ChangeKey(BattleKeyType.KeyRight, Config.ParamEx[0].ToInt(), ChangeKeyReason.Item);
    }
}
