using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3042 : BattleSkillBase
{
    //获得3层武增状态和3层术增状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffWuZeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, GameConst.Battle.BuffShuZeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
}