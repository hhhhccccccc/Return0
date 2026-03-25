using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10115 : BattleHeartMethodBase
{
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        subject.AddNotRecoverGangQiNatural(1);
        subject.AddNotRecoverXuanQiNatural(1);
    }

    public override void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
        BattleSource source = BattleSource.None)
    {
        if (Subject.GetProperty(BattlePropertyType.GangQi) <= 0 && Subject.GetProperty(BattlePropertyType.XuanQi) <= 0)
        {
            Subject.SetBreak(true);
            EnqueueViewModel(Subject.EntityID, MomentViewType.SetBreak);
        }
    }
}