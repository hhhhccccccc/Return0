using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1034 : BattleSkillBase
{
    //获得1层烈命状态
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffLieMing, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    //获得1次行动次数
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddActionTimes(Subject, 1);
    }
}