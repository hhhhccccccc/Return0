using System.Collections.Generic;
using Zenject;

public class Skill3090 : BattleSkillBase
{
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 112034102 - AddBuff
        DoAddBuff(Subject, 20341, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122001102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20011, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}