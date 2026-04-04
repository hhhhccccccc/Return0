using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

//todo 表现
public class BattleHeartMethod10056 : BattleHeartMethodBase
{
    public override void AfterUnitInit()
    {
        var maxHp = Subject.GetProperty(BattlePropertyType.MaxHp);
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var check = maxHp * GetConfigParamFloat(1);
        if (hp >= check)
        {
            Subject.SetProperty(BattlePropertyType.Hp, check);
        }
    }
}