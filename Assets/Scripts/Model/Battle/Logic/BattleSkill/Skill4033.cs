using System.Collections.Generic;
using Zenject;

public class Skill4033 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122029102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20291, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}