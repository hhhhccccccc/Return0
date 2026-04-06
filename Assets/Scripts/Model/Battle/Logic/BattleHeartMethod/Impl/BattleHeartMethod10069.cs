using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10069 : BattleHeartMethodBase
{
    private float SkillDamageRate => GetConfigParamFloat(0);
    private bool InTrigger { get; set; }
    public override void DoDesitionAction(bool isPreDesition)
    {
        var useListCount = Subject.RoundUsedSkillGuid.Count;
        if (useListCount > 0)
        {
            return;
        }
        var skill = Subject.GetSkill();
        if (skill == null)
        {
            return;
        }

        if (BattleUtil.SkillIsKillingStyle(skill.SkillID))
        {
            InTrigger = true;
        }
    }

    public override float AddDamagePct(MomentParamModel paramModel)
    {
        if (InTrigger)
        {
            return SkillDamageRate;
        }

        return 0;
    }


    public override void ClearTempData()
    {
        InTrigger = false;
    }
    
    public override void RoundEnd()
    {
        InTrigger = false;
    }

    protected override void OnHeartMethodRecycle()
    {
        InTrigger = false;
    }
}