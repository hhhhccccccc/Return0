using System.Collections.Generic;
using Zenject;

public class Skill3081 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122021101 - AddBuff
        if (Target != null) DoAddBuff(Target, 20211, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 122014102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20141, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}