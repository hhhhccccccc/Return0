using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10033 : BattleHeartMethodBase
{
    private bool InTrigger { get; set; }
    private bool CanTrigger { get; set; }
    private int Round { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        InTrigger = true;
        CanTrigger = true;
    }

    public override void BeforeReduceHp(float reduceHp)
    {
        if ((Subject.GetProperty(BattlePropertyType.Hp) - reduceHp) / Subject.GetProperty(BattlePropertyType.MaxHp) <=
            GetParamFloat(0) && CanTrigger)
        {
            InTrigger = true;
            CanTrigger = false;
        }
    }

    public override float GetProperty(BattlePropertyType propertyType)
    {
        if (!InTrigger)
        {
            return 0;
        }

        if (propertyType == BattlePropertyType.DefendPct)
        {
            return GetParamFloat(1);
        }

        if (propertyType == BattlePropertyType.BreakPct)
        {
            return GetParamFloat(2);
        }

        return 0;
    }

    public override void RoundEnd()
    {
        base.RoundEnd();
        if (!CanTrigger)
        {
            Round++;
            if (Round >= 2)
            {
                CanTrigger = true;
                Round = 0;
            }
        }
    }

    public override void Recycle()
    {
        InTrigger = false;
        CanTrigger = false;
        Round = 0;
    }
}