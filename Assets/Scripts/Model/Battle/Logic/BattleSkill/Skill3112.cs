using System.Collections.Generic;
using Zenject;

public class Skill3112 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122011103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20111, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}