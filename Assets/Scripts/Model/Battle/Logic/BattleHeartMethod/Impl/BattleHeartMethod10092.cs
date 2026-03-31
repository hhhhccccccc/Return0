using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10092 : BattleHeartMethodBase
{
    private float SkillWelly => GetParamFloat(0);
    public override float GetWellyRateEx(int skillGuid)
    {
        var skill = Subject.GetSkill();
        if (skill.IsRepeat)
        {
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddWelly, SkillWelly);
            return SkillWelly;
        }

        return 0;
    }
}