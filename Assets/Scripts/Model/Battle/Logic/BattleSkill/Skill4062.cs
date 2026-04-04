using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4062 : BattleSkillBase
{
    //对目标施加3层武衰状态和4层玄屏状态和1层失衡状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffWuShuai, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffXuanPing, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffShiHeng, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}