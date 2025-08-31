using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckProperty : BattleMomentCondition
{
    [Inject] private BattleUtil BattleUtil;
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            var checkID = Config.ParamList[1].ToInt();
            var checkType = Config.ParamList[2].ToInt();
            var checkValue = Config.ParamList[3].ToInt();
            var relation = Config.ParamList[4].ToInt();
            float hasValue;
            if (checkType == 1)//值
            {
                hasValue = target.GetProperty((BattlePropertyType)checkID);
            }
            else if (checkType == 2)//百分比
            {
                hasValue = target.GetPropertyPct((BattlePropertyType)checkID);
            }
            else
            {
                return false;
            }
            
            return BattleUtil.CompareValue(hasValue, checkValue, relation);
        }
        
        return false;
    }
}