using System.Collections.Generic;
using Zenject;

public class Skill3080 : BattleSkillBase
{
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 113037104 - AddBuff
        DoAddBuff(Subject, 30371, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122002101 - AddBuff
        if (Target != null) DoAddBuff(Target, 20021, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}