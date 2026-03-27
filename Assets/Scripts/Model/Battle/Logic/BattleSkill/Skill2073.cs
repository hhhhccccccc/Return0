using System.Collections.Generic;
using Zenject;

public class Skill2073 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122030103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20301, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 122030102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20301, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}