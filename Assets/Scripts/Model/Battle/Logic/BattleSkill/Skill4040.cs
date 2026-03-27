using System.Collections.Generic;
using Zenject;

public class Skill4040 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900011 - ChangeActionWheel
        Subject.ChangeActionWheel(-1);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 119001202 - AddBuff
        DoAddBuff(Subject, 90012, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}