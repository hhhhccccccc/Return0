using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10114 : BattleHeartMethodBase
{
    private bool CanTrigger { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        Register<UnitTriggerEndActionEventModel>(OnUnitTriggerEndAction);
    }

    private void OnUnitTriggerEndAction(UnitTriggerEndActionEventModel model)
    {
        if (!CanTrigger)
        {
            return;
        }

        if (Subject.ActionTimes == 0)
        {
            Subject.AddActionTimes(1);
            CanTrigger = false;
        }
    }

    public override void RoundStart()
    {
        base.RoundStart();
        CanTrigger = true;
    }
    
    protected override void OnRecycle()
    {
        CanTrigger = false;
    }
}