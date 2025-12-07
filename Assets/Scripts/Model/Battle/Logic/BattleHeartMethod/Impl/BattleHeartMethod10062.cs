using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10062 : BattleHeartMethodBase
{
    public override void KeyReplace(List<int> result, BattleKeyType keyType)
    {
        if (keyType == BattleKeyType.KeyLeft)
        {
            result.Add((int)BattleKeyType.KeyRight);
        }
        
        if (keyType == BattleKeyType.KeyRight)
        {
            result.Add((int)BattleKeyType.KeyLeft);
        }
    }
}