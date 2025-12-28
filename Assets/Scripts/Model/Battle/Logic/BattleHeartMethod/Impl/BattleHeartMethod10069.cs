using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10069 : BattleHeartMethodBase
{
    private bool InTrigger { get; set; }

    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
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

    public override float GetSkillDamageRate(MomentParamModel paramModel)
    {
        if (InTrigger)
        {
            return GetParamFloat(0);
        }

        return 0;
    }

    public override void RoundEnd()
    {
        InTrigger = false;
        base.RoundEnd();
    }

    protected override void OnRecycle()
    {
        InTrigger = false;
    }
}