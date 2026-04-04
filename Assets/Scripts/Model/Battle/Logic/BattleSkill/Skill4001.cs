using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4001 : BattleSkillBase
{
    //对目标施加4层破绽
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffPoZhan, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
    }
}