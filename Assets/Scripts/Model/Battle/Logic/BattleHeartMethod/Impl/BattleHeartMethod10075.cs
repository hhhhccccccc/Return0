using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

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
        DefendPct = GetConfigParamFloat(0);
        BreakPct = GetConfigParamFloat(1);
    }

    public override void AfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        if (damageType == DamageType.Direct)
        {
            DefendPct -= GetConfigParamFloat(2);
            BreakPct -= GetConfigParamFloat(3);
        }
    }

    public override float GetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
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

    protected override void OnHeartMethodRecycle()
    {
        DefendPct = 0;
        BreakPct = 0;
    }
}