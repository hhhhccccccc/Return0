using System.Collections.Generic;
using Zenject;

public class Skill4002 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900003 - ChangeActionWheel
        Subject.ChangeActionWheel(3);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 162007101 - AddBuff
        // TODO: AddBuff [caster=1, target=6]
    }

}