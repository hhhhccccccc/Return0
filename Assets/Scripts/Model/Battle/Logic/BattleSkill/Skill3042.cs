using System.Collections.Generic;
using Zenject;

public class Skill3042 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111009103 - AddBuff
        DoAddBuff(Subject, 10091, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 111010103 - AddBuff
        DoAddBuff(Subject, 10101, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}