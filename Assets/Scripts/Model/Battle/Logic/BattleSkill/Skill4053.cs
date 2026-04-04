using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4053 : BattleSkillBase
{
    //获得4层禽化身状态
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffQinHuaShen, Subject, 4, null, BattleMomentType.DoDesitionAction);
    }

    //获得2层迅速状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffXunSu, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}