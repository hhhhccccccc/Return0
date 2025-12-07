using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10097 : BattleHeartMethodBase
{
    private bool CanTrigger { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanTrigger = true;
    }

    public override void RoundStart()
    {
        base.RoundStart();
        CanTrigger = true;
    }

    public override void EndAction()
    {
        base.EndAction();
        if (CanTrigger && Subject.ActionTimes == 0)
        {
            BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10021, Subject, GetParamInt(0));
            CanTrigger = false;
        }
    }

    public override void Recycle()
    {
        CanTrigger = false;
        base.Recycle();
    }
}