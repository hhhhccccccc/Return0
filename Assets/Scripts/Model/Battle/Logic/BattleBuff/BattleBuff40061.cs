using cfg;

public class BattleBuff40061 : BattleBuffPotion
{
    protected override void OnSelfActionWheelStart()
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, GetConfigParamFloat(0), BattleSource.Item);
    }
}
