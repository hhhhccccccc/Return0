using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1021 : BattleSkillBase
{
    //行动延迟1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, -1);
    }

    //获得3层心眼状态，获得1次行动次数
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffXinYan, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddActionTimes(Subject, 1);
    }
}