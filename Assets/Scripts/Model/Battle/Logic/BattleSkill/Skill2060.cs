using System.Collections.Generic;
using Zenject;

public class Skill2060 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122007103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20071, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}