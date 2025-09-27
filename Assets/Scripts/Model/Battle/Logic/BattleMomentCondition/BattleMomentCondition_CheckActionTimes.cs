using Zenject;

public class BattleMomentCondition_CheckActionTimes : BattleMomentCondition
{
    [Inject] private BattleUtil BattleUtil { get; set; }
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(1);
        if (target != null)
        {
            var hasCount = target.ActionTimes;
            var checkCount = Config.ParamList[0].ToInt();
            var relation = Config.ParamList[1].ToInt();
            return BattleUtil.CompareValue(hasCount, checkCount, relation);
        }
        
        return false;
    }
}