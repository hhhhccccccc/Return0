using Zenject;

public class BattleMomentCondition_ReturnFalse : BattleMomentCondition
{
    [Inject] private BattleUtil BattleUtil { get; set; }
    protected override bool OnCondition()
    {
        return false;
    }
}