using System.Collections.Generic;
using Zenject;

public class Skill4071 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122013105 - AddBuff
        if (Target != null) DoAddBuff(Target, 20131, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 122010105 - AddBuff
        if (Target != null) DoAddBuff(Target, 20101, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 122002102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20021, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 3400001 - AddActionTimes
        Subject.AddActionTimes(1);
    }

}