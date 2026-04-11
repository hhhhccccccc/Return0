using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2065 : BattleSkillBase
{
    //todo 随机封锁目标两个键直到回合结束
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, 72065, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    //获得3层心眼状态和3层巧增状态
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffXinYan, Subject, 3, null, BattleMomentType.AfterAction);
        DoAddBuff(Subject, GameConst.Battle.BuffQiaoZeng, Subject, 3, null, BattleMomentType.AfterAction);
    }
}