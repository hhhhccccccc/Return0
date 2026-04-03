using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3035 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffDuanJinShi, Subject, 1, null, BattleMomentType.DoDesitionAction);
    }

    //施加2层力衰
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var clashUnit = GetClashUnit(paramModel);
        DoAddBuff(clashUnit, GameConst.Battle.BuffLiShuai, Subject, 2, null, BattleMomentType.DoDesitionAction);
    }
}