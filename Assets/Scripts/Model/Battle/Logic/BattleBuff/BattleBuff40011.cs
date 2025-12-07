using cfg;

public class BattleBuff40011 : BattleBuffPotion
{
    protected override void OnSelfActionWheelStart()
    {
        Subject.ChangeProperty(BattlePropertyType.GangQi, Config.ParamEx[0]);
    }
}
