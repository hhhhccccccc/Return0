using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10085 : BattleHeartMethodBase
{
    public override float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.MaxXuanQiInt)
        {
            return GetParamFloat(0);
        }
        
        if (propertyType == BattlePropertyType.MaxGangQiInt)
        {
            return GetParamFloat(0);
        }

        return 0;
    }
}