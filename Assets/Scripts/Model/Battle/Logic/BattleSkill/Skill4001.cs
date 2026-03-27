using System.Collections.Generic;
using Zenject;

public class Skill4001 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122007104 - AddBuff
        if (Target != null) DoAddBuff(Target, 20071, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
    }

}