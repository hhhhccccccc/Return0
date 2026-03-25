using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10054 : BattleHeartMethodBase
{
    private float SkillDamageRate => GetParamFloat(1);
    public override float GetSkillDamageRate(MomentParamModel paramModel)
    {
        var skill = Subject.GetSkill();
        if (skill == null)
        {
            return 0;
        }
        
        if (skill.GetKeyCostList.Count >= GetParamInt(0))
        {
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddRate, SkillDamageRate);
            return SkillDamageRate;
        }

        return 0;
    }
}