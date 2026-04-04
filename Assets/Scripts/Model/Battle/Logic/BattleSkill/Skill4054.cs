using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4054 : BattleSkillBase
{
    //获得4层祖化身状态
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffZuHuaShen, Subject, 4, null, BattleMomentType.DoDesitionAction);
    }

    //获得2层术增状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffShuZeng, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}