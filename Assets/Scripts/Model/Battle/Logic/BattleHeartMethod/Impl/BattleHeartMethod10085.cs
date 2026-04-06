using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10085 : BattleHeartMethodBase
{
    public override float GetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.MaxXuanQiInt)
        {
            return GetConfigParamFloat(0);
        }
        
        if (propertyType == BattlePropertyType.MaxGangQiInt)
        {
            return GetConfigParamFloat(0);
        }

        return 0;
    }
}