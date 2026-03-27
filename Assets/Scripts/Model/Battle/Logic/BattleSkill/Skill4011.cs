using System.Collections.Generic;
using Zenject;

public class Skill4011 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900012 - ChangeActionWheel
        Subject.ChangeActionWheel(-2);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 4402100 - ClearBuffByType
        // TODO: ClearBuffByType target=2
    }

}