using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4071 : BattleSkillBase
{
    //对目标施加5层武衰状态和5层玄屏状态和2层失衡状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffWuShuai, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffXuanPing, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffShiHeng, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
    
    //获得1次行动次数
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddActionTimes(Subject, 1);
    }
}