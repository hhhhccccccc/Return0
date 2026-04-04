using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4002 : BattleSkillBase
{
    //行动加快3息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, 3);
    }

    //对全体对手施加1层破绽
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var allOppoUnit = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
        foreach (var unit in allOppoUnit)
        {
            DoAddBuff(unit, GameConst.Battle.BuffPoZhan, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
        }
    }
}