using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3055 : BattleSkillBase
{
    //解除自身2个负面状态
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoClearBuffByType(Subject, BuffType.Abnormal, 2);
    }

    //施加1层晕眩状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffXuanYun, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    //获得1层晕眩状态
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffXuanYun, Subject, 1, null, BattleMomentType.AfterAction);
    }
}