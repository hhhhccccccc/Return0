using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10064 : BattleHeartMethodBase
{
    private int ActionTimes => GetParamInt(1);
    private int CumulateCount { get; set; }
    public override void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (changeType == ChangeKeyType.Cost)
        {
            CumulateCount += changeKeyData.Count;
        }
    }

    public override void RoundStart()
    {
        base.RoundStart();
        if (CumulateCount >= GetParamInt(0))
        {
            CumulateCount = 0;
            Subject.AddActionTimes(ActionTimes);
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddActionTimes, ActionTimes);
        }
    }

    protected override void OnRecycle()
    {
        CumulateCount = 0;
    }
}