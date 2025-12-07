using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10059 : BattleHeartMethodBase
{
    public override void KeyReplace(List<int> result, BattleKeyType keyType)
    {
        if (keyType == BattleKeyType.KeyUp)
        {
            result.Add((int)BattleKeyType.KeyDown);
        }
    }
}