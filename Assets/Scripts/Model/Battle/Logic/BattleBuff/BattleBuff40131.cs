using cfg;

public class BattleBuff40131 : BattleBuffPotion
{
    protected override void OnRoundEnd()
    {
        DoAddRandomKey(Subject, GetConfigParamInt(0), ChangeKeyReason.Item);
    }
}
