using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2037 : BattleSkillBase
{
    //施加2层失持状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffShiChi, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}