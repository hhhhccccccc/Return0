using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1032 : BattleSkillBase
{
    protected override int DontBeCounter(MomentParamModel paramModel)
    {
        return 1;
    }
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 4200021 - ChangeTargetToOther
        // TODO: ChangeTargetToOther
    }
    //获得1个随机的键
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 1, ChangeKeyReason.SkillEffect);
    }
}