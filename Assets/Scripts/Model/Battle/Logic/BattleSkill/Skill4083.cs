using System.Collections.Generic;
using Zenject;

public class Skill4083 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111011103 - AddBuff
        DoAddBuff(Subject, 10111, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 3400001 - AddActionTimes
        Subject.AddActionTimes(1);
    }

}