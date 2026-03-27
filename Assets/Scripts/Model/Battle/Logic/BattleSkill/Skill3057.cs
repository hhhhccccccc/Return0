using System.Collections.Generic;
using Zenject;

public class Skill3057 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 111017103 - AddBuff
        DoAddBuff(Subject, 10171, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 4402102 - ClearBuffByType
        // TODO: ClearBuffByType target=2
    }

}