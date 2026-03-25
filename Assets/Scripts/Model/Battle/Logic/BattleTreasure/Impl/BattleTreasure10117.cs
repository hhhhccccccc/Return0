using cfg;

public class BattleTreasure10117 : BattleTreasureBase
{
    protected override void OnRoundStart()
    {
        var finalValue = Subject.ChangeProperty(BattlePropertyType.Hp, GetParamFloat(0), BattleSource.Treasure);
        EnqueueViewModel(Subject.EntityID, MomentViewType.ChangeHp, finalValue);
    }
}


