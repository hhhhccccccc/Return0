using cfg;

public class BattleBuff40111 : BattleBuffPotion
{
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        Subject.AddRandomKey(Config.ParamEx[0].ToInt(), ChangeKeyReason.Item);
    }
}
