using System.Collections.Generic;
using Zenject;

public class Skill1034 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 113040101 - AddBuff
        DoAddBuff(Subject, 30401, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 3400001 - AddActionTimes
        Subject.AddActionTimes(1);
    }

}