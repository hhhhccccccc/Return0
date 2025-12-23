using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10056 : BattleHeartMethodBase
{
    public override void AfterUnitInit()
    {
        var maxHp = Subject.GetProperty(BattlePropertyType.MaxHp);
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var check = maxHp * GetParamFloat(1);
        if (hp >= check)
        {
            Subject.SetProperty(BattlePropertyType.Hp, check);
        }
    }
}