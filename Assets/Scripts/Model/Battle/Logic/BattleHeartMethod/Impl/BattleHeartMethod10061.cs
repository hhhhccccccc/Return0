using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10061 : BattleHeartMethodBase
{
    public override void KeyReplace(List<int> result, BattleKeyType keyType)
    {
        if (keyType == BattleKeyType.KeyDown)
        {
            result.Add((int)BattleKeyType.KeyUp);
        }
    }

    public override float GetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.CleverInt)
        {
            return GetParamFloat(0) + GetParamFloat(1) * Subject.Gr;
        }

        return 0;
    }
}