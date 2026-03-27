using System.Collections.Generic;
using Zenject;

public class Skill3078 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122017102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20171, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}