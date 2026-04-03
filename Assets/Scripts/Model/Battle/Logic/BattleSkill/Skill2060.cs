using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2060 : BattleSkillBase
{
    //施加3层破绽状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffPoZhan, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
}