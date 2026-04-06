using cfg;

public class BattleBuff40011 : BattleBuffPotion
{
    protected override void OnSelfActionWheelStart()
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, GetConfigParamFloat(0), BattleSource.Item);
    }
}
