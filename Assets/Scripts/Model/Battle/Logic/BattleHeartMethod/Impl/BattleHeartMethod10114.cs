using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10112 : BattleHeartMethodBase
{
    private bool CanIgnore { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanIgnore = false;
    }

    public override void RoundStart()
    {
        base.RoundStart();
        CanIgnore = true;
    }

    public override bool CanIgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        if (!CanIgnore)
        {
            return false;
        }
        
        if (paramModel is DamageParamModel model)
        {
            var attackSkillDamageRate = model.GetOtherFinalDamageRate(Subject.EntityID);
            if (attackSkillDamageRate > GetParamFloat(0))
            {
                CanIgnore = false;
                return true;
            }
        }

        return false;
    }

    public override void Recycle()
    {
        CanIgnore = false;
        base.Recycle();
    }
}