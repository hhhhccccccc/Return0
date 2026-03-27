using System.Collections.Generic;
using Zenject;

public class Skill2072 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 119001602 - AddBuff
        DoAddBuff(Subject, 90016, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}