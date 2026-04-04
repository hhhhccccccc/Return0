using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4057 : BattleSkillBase
{
    //全部友方获得1层心眼状态和2层巧增状态
    public override void DoDesitionAction(bool isPreDesition)
    {
        foreach (var unit in BattleManager.GetAllTeamUnit(Subject.EntityID, true))
        {
            DoAddBuff(unit, GameConst.Battle.BuffXinYan, Subject, 1, null, BattleMomentType.DoDesitionAction);
            DoAddBuff(unit, GameConst.Battle.BuffQiaoZeng, Subject, 2, null, BattleMomentType.DoDesitionAction);
        }
    }

    //对目标施加2层武式禁和2层术式禁
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffWuShiJin, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffShuShiJin, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}