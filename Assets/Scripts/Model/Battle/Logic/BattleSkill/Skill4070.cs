using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4070 : BattleSkillBase
{
    //对目标施加4层武衰状态和5层玄屏状态和1层失衡状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffWuShuai, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffXuanPing, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffShiHeng, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
    
    //获得1次行动次数
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddActionTimes(Subject, 1);
    }
}