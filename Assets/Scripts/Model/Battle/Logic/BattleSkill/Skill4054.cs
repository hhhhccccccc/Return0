using System.Collections.Generic;
using Zenject;

public class Skill4054 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 113039104 - AddBuff
        DoAddBuff(Subject, 30391, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111010102 - AddBuff
        DoAddBuff(Subject, 10101, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}