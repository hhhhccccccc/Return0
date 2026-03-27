using System.Collections.Generic;
using Zenject;

public class Skill2053 : BattleSkillBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 117205401 - AddBuff
        DoAddBuff(Subject, 72054, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}