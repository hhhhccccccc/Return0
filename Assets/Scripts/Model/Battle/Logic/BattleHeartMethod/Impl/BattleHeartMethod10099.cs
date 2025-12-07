using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10099 : BattleHeartMethodBase
{
    public override float GetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.MaxGangQiInt)
        {
            return GetParamFloat(0);
        }

        return 0;
    }
}