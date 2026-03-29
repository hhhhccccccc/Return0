using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1048 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        //施加3层龙腾状态
        DoAddBuff(Subject, GameConst.Battle.BuffLongTeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}