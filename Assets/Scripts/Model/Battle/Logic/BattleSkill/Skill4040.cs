using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4040 : BattleSkillBase
{
    //本次行动延迟1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, -1);
    }

    //下次行动的刚炁消耗减少30
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, 90012, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}