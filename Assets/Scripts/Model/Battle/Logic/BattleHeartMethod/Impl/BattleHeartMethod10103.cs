using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10103 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        base.RoundStart();
        if (Subject.GetProperty(BattlePropertyType.XuanQi) >= GetParamFloat(0))
        {
            Subject.AddActionTimes(GetParamInt(1));
        }
    }
}