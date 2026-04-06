using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10092 : BattleHeartMethodBase
{
    private float SkillWelly => GetConfigParamFloat(0);
    public override float GetWellyRateEx(int skillGuid)
    {
        var skill = Subject.GetSkill();
        if (skill.IsRepeat)
        {
            return SkillWelly;
        }

        return 0;
    }
}