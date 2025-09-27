using Zenject;

public class BattleMomentCondition_CheckKeyCount : BattleMomentCondition
{
    [Inject] private BattleUtil BattleUtil { get; set; }
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var hasCount = target.GetKeyCount();
            var checkCount = Config.ParamList[1].ToInt();
            var relation = Config.ParamList[2].ToInt();
            return BattleUtil.CompareValue(hasCount, checkCount, relation);
        }
        
        return false;
    }
}