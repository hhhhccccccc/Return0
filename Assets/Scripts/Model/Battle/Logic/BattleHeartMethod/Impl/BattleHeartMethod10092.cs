using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10092 : BattleHeartMethodBase
{
    public override float GetSkillWellyRate(int skillGuid)
    {
        var skill = Subject.GetSkill();
        if (skill.IsRepeat)
        {
            return GetParamFloat(0);
        }

        return 0;
    }
}