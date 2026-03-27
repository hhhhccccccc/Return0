using System.Collections.Generic;
using Zenject;

public class Skill2037 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122028102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20281, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}