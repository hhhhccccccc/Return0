using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckDamageWithProperty : BattleMomentCondition
{
    [Inject] private BattleUtil BattleUtil;
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null && ParamModel is DamageParamModel model)
        {
            var propertyID = Config.ParamList[1].ToInt();
            var propertyValue = target.GetProperty((BattlePropertyType)propertyID);
            if (Config.ParamList[2].ToInt() == 0) //值
            {
                var checkValue = propertyValue;
                var relation = Config.ParamList[3].ToInt();
                return BattleUtil.CompareValue(model.GetSelfAttackHpValue(Subject.EntityID), checkValue, relation);
            }
            
            if (Config.ParamList[2].ToInt() == 1) //百分比
            {
                var checkValue = Config.ParamList[3] * propertyValue;
                var relation = Config.ParamList[4].ToInt();
                return BattleUtil.CompareValue(model.GetSelfAttackHpValue(Subject.EntityID), checkValue, relation);
            }
        }
        return false;
    }
}