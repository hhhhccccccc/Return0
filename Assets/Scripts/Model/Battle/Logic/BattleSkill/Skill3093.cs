using System;
using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3093 : BattleSkillBase
{
    protected override float SkillAddDamageRate()
    {
        var hp = Target.GetProperty(BattlePropertyType.Hp);
        var hpMax = Target.GetProperty(BattlePropertyType.MaxHp);
        return hp / hpMax;
    }
}