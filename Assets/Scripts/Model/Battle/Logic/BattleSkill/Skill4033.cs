using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4033 : BattleSkillBase
{
    //对目标施加2层共生状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffGongSheng, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}