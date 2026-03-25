using cfg;

public class BattleTreasure10126 : BattleTreasureBase
{
    protected override void OnBattleEnd()
    {
        var finalValue = Subject.ChangeProperty(BattlePropertyType.Hp, Subject.GetProperty(BattlePropertyType.MaxHp) * GetParamFloat(0), BattleSource.Treasure);
        EnqueueViewModel(Subject.EntityID, MomentViewType.ChangeHp, finalValue);
    }
}


