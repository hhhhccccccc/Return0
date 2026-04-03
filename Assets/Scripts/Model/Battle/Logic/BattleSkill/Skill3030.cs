using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3030 : BattleSkillBase
{
    //施加2层力衰
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffLiShuai, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    //玄炁+10
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
    }
}