using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10104 : BattleHeartMethodBase
{
    public override void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (Subject.GetAllKeyCount() <= 0)
        {
            Subject.AddRandomKey(GetParamInt(0), ChangeKeyReason.HeartMethodEffect);
        }
    }
}