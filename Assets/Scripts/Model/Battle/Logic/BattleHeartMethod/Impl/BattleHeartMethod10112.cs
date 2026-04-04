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

    public override bool IgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        if (!CanIgnore)
        {
            return false;
        }
        
        if (paramModel is DamageParamModel model)
        {
            var attackSkillDamageRate = model.GetOtherFinalDamageWelly(Subject.EntityID);
            if (attackSkillDamageRate > GetConfigParamFloat(0))
            {
                CanIgnore = false;
                EnqueueViewModel(Subject.EntityID, MomentViewType.IgnoreSkillDirectDamage);
                return true;
            }
        }

        return false;
    }

    protected override void OnHeartMethodRecycle()
    {
        CanIgnore = false;
    }
}