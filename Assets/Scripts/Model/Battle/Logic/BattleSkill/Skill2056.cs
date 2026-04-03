using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2056 : BattleSkillBase
{
    //恢复本回合受到伤害30%的体
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoHealHp(Subject, 0.3f * Subject.RoundBeDamageValue, BattleSource.Skill);
    }

    //玄炁+10
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
    }
}