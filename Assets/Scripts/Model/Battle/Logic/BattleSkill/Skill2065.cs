using System.Collections.Generic;
using Zenject;

public class Skill2065 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 127206501 - AddBuff
        if (Target != null) DoAddBuff(Target, 72065, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 111001103 - AddBuff
        DoAddBuff(Subject, 10011, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 111011103 - AddBuff
        DoAddBuff(Subject, 10111, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}