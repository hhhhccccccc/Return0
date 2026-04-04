using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10084 : BattleHeartMethodBase
{
    public override float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.MaxXuanQiInt)
        {
            return GetConfigParamFloat(0);
        }

        return 0;
    }
}