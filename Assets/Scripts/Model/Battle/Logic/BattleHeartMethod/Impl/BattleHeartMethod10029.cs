using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10029 : BattleHeartMethodBase
{
    public override (float, float) ChangeResourceCost(float gangQiCost, float xuanQiCost)
    {
        return (gangQiCost, Math.Max(xuanQiCost - GetParamFloat(1), 0));
    }
}