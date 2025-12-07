using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10009 : BattleHeartMethodBase
{
    public override float GetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.BreakPct)
        {
            return GetParamFloat(0);
        }

        if (propertyType == BattlePropertyType.DefendPct)
        {
            return GetParamFloat(1);
        }

        return 0;
    }
}