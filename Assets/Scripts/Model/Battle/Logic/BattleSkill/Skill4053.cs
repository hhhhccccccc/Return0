using System.Collections.Generic;
using Zenject;

public class Skill4053 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 113038104 - AddBuff
        DoAddBuff(Subject, 30381, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111004102 - AddBuff
        DoAddBuff(Subject, 10041, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}