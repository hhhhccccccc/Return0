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
        InTrigger = false;
        CanTrigger = true;
    }

    public override bool CheckReCalculateDamage(MomentParamModel paramModel)
    {
        if (!CanTrigger)
        {
            return false;
        }
        
        if (paramModel is DamageParamModel model)
        {
            if ((Subject.GetProperty(BattlePropertyType.Hp) - model.GetOtherHpValue(Subject.EntityID)) / Subject.GetProperty(BattlePropertyType.MaxHp) <=
                GetParamFloat(0))
            {
                InTrigger = true;
                CanTrigger = false;
                return true;
            }
        }

        return false;
    }

    public override float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
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
        InTrigger = false;
    }

    public override void Recycle()
    {
        InTrigger = false;
        CanTrigger = false;
        Round = 0;
    }
}