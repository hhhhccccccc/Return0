using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4083 : BattleSkillBase
{
    //todo 本次行动不影响状态的存续
    
    //获得3层稳势
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffWenShi, Subject, 3, null, BattleMomentType.DoDesitionAction);
    }

    //获得3层巧增
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffQiaoZeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    //获得1次行动次数，下次行动加快10息
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddActionTimes(Subject, 1);
        DoAddBuff(Subject, 90018, Subject, 2, null, BattleMomentType.AfterAction);
    }
}