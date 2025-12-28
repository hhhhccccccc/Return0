using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10143 : BattleHeartMethodBase
{
    private float Accumulate { get; set; }
    private float Single { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        Single = subject.GetProperty(BattlePropertyType.BasicMaxHp) * GetParamFloat(0);
        Accumulate = 0;
    }

    public override void ReduceHp(float reduceHp, DamageType damageType, int attackID)
    {
        Accumulate += reduceHp;
        while (Accumulate >= Single)
        {
            Accumulate -= Single;
            Subject.AddActionTimes(GetParamInt(0));
        }
    }

    protected override void OnRecycle()
    {
        Accumulate = 0;
        Single = 0;
    }
}