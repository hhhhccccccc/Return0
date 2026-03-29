using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1031 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900011 - ChangeActionWheel
        DoAddActionTimes(Subject, -1);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 119000502 - AddBuff
        DoAddBuff(Subject, 90005, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}