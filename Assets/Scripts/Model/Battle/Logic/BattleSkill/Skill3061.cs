using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3061 : BattleSkillBase
{
    //招式的刚炁消耗转为当前70%，至多70，本次行动延迟2息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.GangQi, 0.7f, 70);
        DoChangeActionWheel(Subject, -2);
    }

    //本回合未受到过杀式直接攻击则造成的伤害增加40%
    public override float AddDamagePct(MomentParamModel paramModel)
    {
        if (Subject.RoundBeDirectDamageTimes <= 0)
        {
            return 0.4f;
        }

        return 0;
    }
}