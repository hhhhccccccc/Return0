using System.Linq;
using Zenject;

public class BattleMomentCondition_CheckNoSameKeyCount : BattleMomentCondition
{
    [Inject] private BattleUtil BattleUtil { get; set; }
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var noSameCount = target.GetAllKeyTypeList().Distinct().Count();
            var checkCount = Config.ParamList[1].ToInt();
            var relation = Config.ParamList[2].ToInt();
            return BattleUtil.CompareValue(noSameCount, checkCount, relation);
        }
        
        return false;
    }
}