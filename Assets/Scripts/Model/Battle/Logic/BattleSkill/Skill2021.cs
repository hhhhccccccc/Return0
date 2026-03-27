using System.Collections.Generic;
using Zenject;

public class Skill2021 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 112014103 - AddBuff
        DoAddBuff(Subject, 20141, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122014102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20141, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}