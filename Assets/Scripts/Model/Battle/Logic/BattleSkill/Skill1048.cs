using System.Collections.Generic;
using Zenject;

public class Skill1048 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111021103 - AddBuff
        DoAddBuff(Subject, 10211, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}