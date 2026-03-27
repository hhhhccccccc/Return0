using System.Collections.Generic;
using Zenject;

public class Skill4010 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122021105 - AddBuff
        if (Target != null) DoAddBuff(Target, 20211, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
    }

}