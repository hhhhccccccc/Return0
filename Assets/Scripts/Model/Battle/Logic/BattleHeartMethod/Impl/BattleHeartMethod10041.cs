using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10041 : BattleHeartMethodBase
{
    private float SkillRate => GetConfigParamFloat(0);
    public override float AddDamagePct(MomentParamModel paramModel)
    {
        var skill = Subject.GetSkill();
        if (skill == null)
        {
            return 0;
        }
        
        if (skill.GetSKillType != SkillType.ArtKilling)
        {
            return 0;
        }
        
        EnqueueViewModel(Subject.EntityID, MomentViewType.AddRate, SkillRate);
        return SkillRate;
    }
}