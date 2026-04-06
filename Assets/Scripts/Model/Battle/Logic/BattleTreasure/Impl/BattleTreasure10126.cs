using cfg;

public class BattleTreasure10126 : BattleTreasureBase
{
    protected override void OnBattleEnd()
    {
        DoChangeProperty(Subject, BattlePropertyType.Hp, Subject.GetProperty(BattlePropertyType.MaxHp) * GetConfigParamFloat(0), BattleSource.Treasure);
    }
}


