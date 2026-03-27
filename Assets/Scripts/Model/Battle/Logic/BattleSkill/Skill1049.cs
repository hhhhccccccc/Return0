using System.Collections.Generic;
using Zenject;

public class Skill1049 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 4401201 - ClearBuffByType
        DoClearBuffByType(Subject, 2, 1);
        // 效果: 111005105 - AddBuff
        DoAddBuff(Subject, 10051, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 111006105 - AddBuff
        DoAddBuff(Subject, 10061, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 111004103 - AddBuff
        DoAddBuff(Subject, 10041, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}