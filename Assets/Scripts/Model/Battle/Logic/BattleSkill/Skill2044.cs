using System.Collections.Generic;
using Zenject;

public class Skill2044 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122020101 - AddBuff
        if (Target != null) DoAddBuff(Target, 20201, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}