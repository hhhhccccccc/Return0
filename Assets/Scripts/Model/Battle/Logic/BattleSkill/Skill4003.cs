using System.Collections.Generic;
using Zenject;

public class Skill4003 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 4401200 - ClearBuffByType
        DoClearBuffByType(Subject, 2, 0);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 400002 - AddRandomKey
        Subject.AddRandomKey(2, (ChangeKeyReason)4);
    }

    public override void SkillEnd()
    {
        base.SkillEnd();
        // 效果: 119001302 - AddBuff
        DoAddBuff(Subject, 90013, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}