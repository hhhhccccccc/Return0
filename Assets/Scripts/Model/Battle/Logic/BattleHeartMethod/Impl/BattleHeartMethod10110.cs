using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10110 : BattleHeartMethodBase
{
    public bool CanTrigger { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanTrigger = true;
    }

    public override void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (Subject.GetAllKeyCount() <= 0)
        {
            var max = Subject.GetKeyPropertyMax();
            Subject.AddRandomKey(max, ChangeKeyReason.HeartMethodEffect);
        }
    }

    protected override void OnRecycle()
    {
        CanTrigger = false;
    }
}