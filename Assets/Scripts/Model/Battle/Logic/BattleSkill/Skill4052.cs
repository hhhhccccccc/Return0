using System.Collections.Generic;
using Zenject;

public class Skill4052 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 113037104 - AddBuff
        DoAddBuff(Subject, 30371, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111009102 - AddBuff
        DoAddBuff(Subject, 10091, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}