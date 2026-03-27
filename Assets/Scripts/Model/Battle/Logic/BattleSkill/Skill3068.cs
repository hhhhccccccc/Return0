using System.Collections.Generic;
using Zenject;

public class Skill3068 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 122007102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20071, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122015101 - AddBuff
        if (Target != null) DoAddBuff(Target, 20151, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}