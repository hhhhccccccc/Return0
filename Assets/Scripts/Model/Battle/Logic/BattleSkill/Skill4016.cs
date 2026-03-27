using System.Collections.Generic;
using Zenject;

public class Skill4016 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 3400001 - AddActionTimes
        Subject.AddActionTimes(1);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 113028101 - AddBuff
        DoAddBuff(Subject, 30281, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}