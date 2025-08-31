using Zenject;

public class BattleMomentCondition_CheckBuff : BattleMomentCondition
{
    [Inject] private BattleUtil BattleUtil;
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var checkBuff = Config.ParamList[1].ToInt();
            var hasCount = target.GetBuffCountByID(checkBuff);
            var checkLevel = Config.ParamList[2].ToInt();
            var relation = Config.ParamList[3].ToInt();
            return BattleUtil.CompareValue(hasCount, checkLevel, relation);
        }
        
        return false;
    }
}