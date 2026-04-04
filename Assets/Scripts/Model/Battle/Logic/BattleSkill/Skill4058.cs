using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4058 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        foreach (var unit in BattleManager.GetAllAliveUnit())
        {
            DoAddBuff(unit, GameConst.Battle.BuffHuiBi, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
        }
    }
}