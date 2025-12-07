using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10028 : BattleHeartMethodBase
{
    public override (float, float) ChangeResourceCost(float gangQiCost, float xuanQiCost)
    {
        return (Math.Max(gangQiCost - GetParamFloat(1), 0), xuanQiCost);
    }
}