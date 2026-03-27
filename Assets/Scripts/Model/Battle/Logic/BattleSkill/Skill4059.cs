using System.Collections.Generic;
using Zenject;

public class Skill4059 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111009103 - AddBuff
        DoAddBuff(Subject, 10091, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 121009103 - AddBuff
        if (Target != null) DoAddBuff(Target, 10091, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 111014102 - AddBuff
        DoAddBuff(Subject, 10141, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 121014102 - AddBuff
        if (Target != null) DoAddBuff(Target, 10141, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}