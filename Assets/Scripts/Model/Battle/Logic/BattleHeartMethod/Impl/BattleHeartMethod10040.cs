using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10040 : BattleHeartMethodBase
{
    private float SkillRate => GetParamFloat(0);
    public override float GetDamagePctSum(MomentParamModel paramModel)
    {
        var skill = Subject.GetSkill();
        if (skill == null)
        {
            return 0;
        }
        
        if (skill.GetSKillType != SkillType.PowerKilling)
        {
            return 0;
        }

        EnqueueViewModel(Subject.EntityID, MomentViewType.AddRate, SkillRate);
        return SkillRate;
    }
}