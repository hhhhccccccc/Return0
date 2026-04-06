using cfg;

public class BattleBuff40051 : BattleBuffPotion
{
    protected override void OnRoundStart()
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, GetConfigParamFloat(0) * LayerCount, BattleSource.Item);
    }
}
