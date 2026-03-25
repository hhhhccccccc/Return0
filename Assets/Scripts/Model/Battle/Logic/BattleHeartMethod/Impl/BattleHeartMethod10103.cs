using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10103 : BattleHeartMethodBase
{
    private int Times => GetParamInt(1);
    public override void RoundStart()
    {
        base.RoundStart();
        if (Subject.GetProperty(BattlePropertyType.XuanQi) >= GetParamFloat(0))
        {
            Subject.AddActionTimes(Times);
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddActionTimes, Times);
        }
    }
}