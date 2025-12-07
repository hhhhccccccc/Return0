using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10096 : BattleHeartMethodBase
{
    public override float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        var skill = Subject.GetSkill();
        if (propertyType == BattlePropertyType.DefendPct && skill.IsInAction)
        {
            return GetParamFloat(0);
        }
        
        if (propertyType == BattlePropertyType.BreakPct && skill.IsInAction)
        {
            return GetParamFloat(1);
        }

        return 0;
    }
}