using cfg;
using UnityEngine;
using Zenject;

public class BattleMomentCondition_CheckBeDamageHpReduce : BattleMomentCondition
{
    protected override bool OnCondition()
    {
        var target = GetUnitByParamID(Config.ParamList[0]);
        if (target != null)
        {
            if (ParamModel is DamageParamModel model)
            {
                return model.HitHpValue > 0;
            }
        }
        
        return false;
    }
}