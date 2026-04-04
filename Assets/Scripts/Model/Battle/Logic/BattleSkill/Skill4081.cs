using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4081 : BattleSkillBase
{
    //使目标恢复20%已损体并消除2个减益状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var hpDelta = Target.GetProperty(BattlePropertyType.MaxHp) - Target.GetProperty(BattlePropertyType.Hp);
        hpDelta *= 0.2f;
        DoHealHp(Target, hpDelta, BattleSource.Skill);
        DoClearBuffByType(Target, BuffType.Abnormal, 2);
    }
}