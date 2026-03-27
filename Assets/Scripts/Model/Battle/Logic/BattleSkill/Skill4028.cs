using System.Collections.Generic;
using Zenject;

public class Skill4028 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 11002100 - RemoveBuff
        // TODO: RemoveBuff
        // 效果: 111016101 - AddBuff
        DoAddBuff(Subject, 10161, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}