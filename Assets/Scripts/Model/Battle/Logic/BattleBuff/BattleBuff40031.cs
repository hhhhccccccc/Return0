using cfg;

public class BattleBuff40031 : BattleBuffPotion
{
    protected override void OnRoundStart()
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, GetConfigParamFloat(0) * LayerCount, BattleSource.Item);
    }
}
