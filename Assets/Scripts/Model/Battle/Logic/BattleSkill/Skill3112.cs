using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3112 : BattleSkillBase
{
    //施加3层力衰
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffLiShuai, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
}