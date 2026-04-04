using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4013 : BattleSkillBase
{
    //本次行动加快3息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, 3);
    }

    //获得3层敷宵剑
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffFuXiaoJian, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
}