using System.Collections.Generic;
using Zenject;

public class Skill4042 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111006105 - AddBuff
        DoAddBuff(Subject, 10061, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 117001 - ChangeProperty
        // TODO: ChangeProperty propType=20093
        // 效果: 118001 - ChangeProperty
        // TODO: ChangeProperty propType=20083
    }

}