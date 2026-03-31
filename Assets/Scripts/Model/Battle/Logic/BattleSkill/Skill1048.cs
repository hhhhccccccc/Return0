using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1048 : BattleSkillBase
{
    //施加3层龙腾状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffLongTeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
}