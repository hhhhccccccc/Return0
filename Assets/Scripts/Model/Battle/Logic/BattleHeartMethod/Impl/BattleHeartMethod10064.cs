using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10064 : BattleHeartMethodBase
{
    private int CumulateCount { get; set; }
    public override void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason)
    {
        if (reason == ChangeKeyReason.SkillCost)
        {
            CumulateCount += changeKeyData.Count;
        }
    }

    public override void RoundStart()
    {
        base.RoundStart();
        if (CumulateCount >= GetParamInt(0))
        {
            Subject.AddActionTimes(1);
            CumulateCount = 0;
        }
    }

    public override void Recycle()
    {
        CumulateCount = 0;
        base.Recycle();
    }
}