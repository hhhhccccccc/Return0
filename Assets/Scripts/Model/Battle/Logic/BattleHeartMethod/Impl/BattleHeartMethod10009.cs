using System.Collections.Generic;
using cfg;
using Zenject;

//todo 表现
public class BattleHeartMethod10009 : BattleHeartMethodBase
{
    public override float GetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.BreakPct)
        {
            return GetConfigParamFloat(0);
        }

        if (propertyType == BattlePropertyType.DefendPct)
        {
            return GetConfigParamFloat(1);
        }

        return 0;
    }
}