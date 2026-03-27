using System.Collections.Generic;
using Zenject;

public class Skill4013 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900003 - ChangeActionWheel
        Subject.ChangeActionWheel(3);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 113007103 - AddBuff
        DoAddBuff(Subject, 30071, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}