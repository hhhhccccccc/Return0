using System;
using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3089 : BattleSkillBase
{
    //根据体的百分比损失1：2百分比增加至多100%伤害，增伤不会低于毒瘴状态层数*10%
    public override float AddDamagePct(MomentParamModel paramModel)
    {
        var buff = Subject.GetBuff(GameConst.Battle.BuffDuZhang);
        var count = buff?.LayerCount ?? 0;
        var hp = Subject.GetProperty(BattlePropertyType.Hp);
        var hpMax = Subject.GetProperty(BattlePropertyType.MaxHp);
        var pct = hp / hpMax;
        var addValue = Math.Max((1 - pct) * 2, count * 0.1f);
        return Math.Min(addValue, 1);
    }
}