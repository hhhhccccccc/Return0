using cfg;

public class BattleBuff40061 : BattleBuffPotion
{
    protected override void OnSelfActionWheelStart()
    {
        Subject.ChangeProperty(BattlePropertyType.XuanQi, Config.ParamEx[0]);
    }
}
