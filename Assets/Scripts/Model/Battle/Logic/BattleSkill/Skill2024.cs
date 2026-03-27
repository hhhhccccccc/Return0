using System.Collections.Generic;
using Zenject;

public class Skill2024 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122009103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20091, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 122010103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20101, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}