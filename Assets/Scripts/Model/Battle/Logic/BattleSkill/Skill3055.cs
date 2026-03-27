using System.Collections.Generic;
using Zenject;

public class Skill3055 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 4401202 - ClearBuffByType
        DoClearBuffByType(Subject, 2, 2);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122032101 - AddBuff
        if (Target != null) DoAddBuff(Target, 20321, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 112032101 - AddBuff
        DoAddBuff(Subject, 20321, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}