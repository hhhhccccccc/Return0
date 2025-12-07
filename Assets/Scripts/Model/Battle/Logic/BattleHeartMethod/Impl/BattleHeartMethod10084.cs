using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10084 : BattleHeartMethodBase
{
    public override float GetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.MaxXuanQiInt)
        {
            return GetParamFloat(0);
        }

        return 0;
    }
}