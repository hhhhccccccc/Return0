using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckPropertyCompare : BattleMomentCondition
{
    [Inject] private BattleUtil BattleUtil;
    protected override bool OnCondition()
    {
        var target1 = GetUnitByParamID(Config.ParamList[0]);
        var target2 = GetUnitByParamID(Config.ParamList[2]);
        if (target1 != null && target2 != null)
        {
            var value1 = target1.GetProperty((BattlePropertyType)Config.ParamList[1].ToInt());
            var value2 = target2.GetProperty((BattlePropertyType)Config.ParamList[3].ToInt());
            var relation = Config.ParamList[4].ToInt();
            return BattleUtil.CompareValue(value1, value2, relation);
        }
        
        return false;
    }
}