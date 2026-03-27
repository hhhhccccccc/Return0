using System.Collections.Generic;
using Zenject;

public class Skill2027 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900011 - ChangeActionWheel
        Subject.ChangeActionWheel(-1);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 5000201 - RemoveRandomKey
        // TODO: RemoveRandomKey
    }

}