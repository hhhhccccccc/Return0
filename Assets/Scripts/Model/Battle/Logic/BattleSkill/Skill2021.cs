using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2021 : BattleSkillBase
{
    //获得3层术衰
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffShuShuai, Subject, 3, null, BattleMomentType.DoDesitionAction);
    }

    //施加2层术衰
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffShuShuai, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}