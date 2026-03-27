using System.Collections.Generic;
using Zenject;

public class Skill3082 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122020102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20201, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}