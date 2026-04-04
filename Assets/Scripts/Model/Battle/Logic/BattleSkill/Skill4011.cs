using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4011 : BattleSkillBase
{
    //本次行动延迟2息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, -2);
    }

    //清除目标全部增益状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoClearBuffByType(Target, BuffType.Gain, 0);
    }
}