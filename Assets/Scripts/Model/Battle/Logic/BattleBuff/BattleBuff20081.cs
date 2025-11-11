using System;
using System.Collections.Generic;
using cfg;

public class BattleBuff20081 : BattleBuffBase
{
    protected override void OnKeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason)
    {
        if (keyType == BattleKeyType.KeyDown || keyType == BattleKeyType.KeyUp || keyType == BattleKeyType.KeyLeft ||
            keyType == BattleKeyType.KeyRight)
        {
            TriggerBuffMomentByCount(Math.Abs(changeKeyData.Count), null);
        }
    }
}
