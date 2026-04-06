using cfg;

public class BattleBuff40041 : BattleBuffPotion
{
    protected override void OnSelfActionWheelStart()
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, GetConfigParamFloat(0) * LayerCount, BattleSource.Item);
    }
}
