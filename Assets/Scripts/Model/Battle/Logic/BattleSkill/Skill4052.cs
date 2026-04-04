using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4052 : BattleSkillBase
{
    //获得4层兽化身状态
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffShouHuaShen, Subject, 4, null, BattleMomentType.DoDesitionAction);
    }

    //获得2层武增状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}