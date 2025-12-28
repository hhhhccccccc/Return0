using cfg;

public class BattleTreasure10117 : BattleTreasureBase
{
    protected override void OnRoundStart()
    {
        Subject.ChangeProperty(BattlePropertyType.Hp, GetParamFloat(0), BattleSource.Treasure);
    }
}


