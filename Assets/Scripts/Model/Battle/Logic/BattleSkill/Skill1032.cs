using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1032 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 1;
    }
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 4200021 - ChangeTargetToOther
        // TODO: ChangeTargetToOther
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 1, ChangeKeyReason.SkillEffect);
    }
}