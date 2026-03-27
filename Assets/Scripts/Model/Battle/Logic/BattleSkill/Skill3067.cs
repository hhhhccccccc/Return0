using System.Collections.Generic;
using Zenject;

public class Skill3067 : BattleSkillBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 111004102 - AddBuff
        DoAddBuff(Subject, 10041, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}