using System;
using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3089 : BattleSkillBase
{
    private const int BuffID = 20341;
    private const float MinAddPct = 0.1f;
    protected override float SkillAttackAddDamage()
    {
        var buff = Subject.GetBuff(BuffID);
        var count = buff?.LayerCount ?? 0;
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var hpMax = Subject.GetProperty(BattlePropertyType.MaxHp);
        var pct = hp / hpMax;
        var addValue = Math.Min((1 - pct) * 2, 1);
        return Math.Max(addValue, count * MinAddPct);
    }
}