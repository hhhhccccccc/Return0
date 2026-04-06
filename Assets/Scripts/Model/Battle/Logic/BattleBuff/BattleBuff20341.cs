using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20341 : BattleBuffBase
{
    protected override void OnRoundEnd()
    {
        var value = GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr;
        DoChangeProperty(Subject, BattlePropertyType.MaxHpInt, -value, BattleSource.Buff);
        DoHealHp(Subject, value, BattleSource.Buff);
    }
}
