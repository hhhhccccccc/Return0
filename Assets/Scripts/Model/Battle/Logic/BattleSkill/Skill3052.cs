using System.Collections.Generic;
using Zenject;

public class Skill3052 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900001 - ChangeActionWheel
        Subject.ChangeActionWheel(1);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122002101 - AddBuff
        if (Target != null) DoAddBuff(Target, 20021, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}