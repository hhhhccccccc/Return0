using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10060 : BattleHeartMethodBase
{
    public override void ConvertChangeKey(ref BattleKeyType keyType, int count)
    {
        if (keyType == BattleKeyType.KeyLeft && count > 0)
        {
            keyType = BattleKeyType.KeyRight;
        }
    }
}