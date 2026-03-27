using System.Collections.Generic;
using Zenject;

public class Skill2059 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 5500001 - AddRandomBuff
        // TODO: AddRandomBuff
        // 效果: 122016101 - AddBuff
        if (Target != null) DoAddBuff(Target, 20161, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}