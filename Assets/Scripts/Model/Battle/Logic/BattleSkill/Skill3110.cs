using System.Collections.Generic;
using Zenject;

public class Skill3110 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122016102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20161, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}