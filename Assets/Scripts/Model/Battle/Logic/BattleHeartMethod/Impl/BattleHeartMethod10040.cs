using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10040 : BattleHeartMethodBase
{
    public override float AddSkillDamageRate(int skillGuid)
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

        return GetParamFloat(0);
    }
}