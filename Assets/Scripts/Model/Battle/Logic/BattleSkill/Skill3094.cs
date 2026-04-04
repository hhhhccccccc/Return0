using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill3094 : BattleSkillBase
{
    //目标体低于折前伤害则不造成伤害而是直接扣除全部体
    public override bool IsTrueDamage(DamageParamModel model)
    {
        var other = GetOtherUnit(model);
        var targetHp = other.GetProperty(BattlePropertyType.Hp);
        return targetHp <= model.GetSelfAttackTruthDamageValue(Subject.EntityID);
    }
} 