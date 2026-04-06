using System;
using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3093 : BattleSkillBase
{
    //根据目标当前体百分比等量提升至多100%伤害
    public override float AddDamagePct(MomentParamModel paramModel)
    {
        var other = GetOtherUnit(paramModel);
        var hp = other.GetProperty(BattlePropertyType.Hp);
        var hpMax = other.GetProperty(BattlePropertyType.MaxHp);
        return hp / hpMax;
    }
}