using System.Collections.Generic;
using Zenject;

public class Skill3113 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122012103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20121, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}