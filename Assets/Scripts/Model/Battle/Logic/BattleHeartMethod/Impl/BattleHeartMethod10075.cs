using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

//todo 表现
public class BattleHeartMethod10075 : BattleHeartMethodBase
{
    private float DefendPct { get; set; }
    private float BreakPct { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        DefendPct = 0;
        BreakPct = 0;
    }

    public override void RoundStart()
    {
        base.RoundStart();
        DefendPct = GetParamFloat(0);
        BreakPct = GetParamFloat(1);
    }

    public override void ReduceHp(float reduceHp, DamageType damageType, int attackID)
    {
        if (damageType == DamageType.Direct)
        {
            DefendPct -= GetParamFloat(2);
            BreakPct -= GetParamFloat(3);
        }
    }

    public override float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.DefendPct)
        {
            return DefendPct;
        }

        if (propertyType == BattlePropertyType.BreakPct)
        {
            return BreakPct;
        }

        return 0;
    }

    protected override void OnRecycle()
    {
        DefendPct = 0;
        BreakPct = 0;
    }
}