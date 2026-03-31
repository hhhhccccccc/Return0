using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1031 : BattleSkillBase
{
    //本次行动延迟1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddActionTimes(Subject, -1);
    }

    //todo 下次行动的玄炁消耗减少30
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, 90005, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}