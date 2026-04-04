using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4016 : BattleSkillBase
{
    //获得1次行动次数
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddActionTimes(Subject, 1);
    }

    //获得燃息1层状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffRanXi, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
    
    //todo 本次行动不影响状态的存续
}