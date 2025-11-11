using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleBuff20341 : BattleBuffBase
{
    [Inject] private BattleManager BattleManager { get; set; }

    protected override void OnRoundEnd()
    {
        var value = Config.ParamEx[0] + Config.ParamEx[1] * Subject.Gr;
        Subject.ChangeProperty(BattlePropertyType.MaxHpInt, -value);
        Subject.HealHp(value, BattleSource.Buff);
    }
}
