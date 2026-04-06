using cfg;

public class BattleTreasure10117 : BattleTreasureBase
{
    protected override void OnRoundStart()
    {
        DoChangeProperty(Subject, BattlePropertyType.Hp, GetConfigParamFloat(0), BattleSource.Treasure);
    }
}


