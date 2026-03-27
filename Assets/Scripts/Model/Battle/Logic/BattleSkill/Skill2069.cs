using System.Collections.Generic;
using Zenject;

public class Skill2069 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 4401201 - ClearBuffByType
        DoClearBuffByType(Subject, 2, 1);
    }

    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 111017101 - AddBuff
        DoAddBuff(Subject, 10171, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}