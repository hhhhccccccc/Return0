using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3113 : BattleSkillBase
{
    //施加3层技衰
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffJiShuai, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
}