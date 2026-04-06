using cfg;

public class BattleBuff40111 : BattleBuffPotion
{
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, GetConfigParamInt(0), ChangeKeyReason.Item);
    }
}
