using Zenject;
using System.Linq;
using cfg;

public class BattleMomentCondition_CheckBuffTypeCount : BattleMomentCondition
{
    [Inject] private BattleUtil BattleUtil { get; set; }
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var hasCount = target.GetBuffList().Count(buff => buff.BuffType == (BuffType)(Config.ParamList[1].ToInt()));
            var checkCount = Config.ParamList[2].ToInt();
            var relation = Config.ParamList[3].ToInt();
            return BattleUtil.CompareValue(hasCount, checkCount, relation);
        }
        
        return false;
    }
}