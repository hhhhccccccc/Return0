using System.Collections.Generic;
using Zenject;

public class Skill3107 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122032101 - AddBuff
        if (Target != null) DoAddBuff(Target, 20321, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}