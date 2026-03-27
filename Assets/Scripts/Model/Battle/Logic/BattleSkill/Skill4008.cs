using System.Collections.Generic;
using Zenject;

public class Skill4008 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900001 - ChangeActionWheel
        Subject.ChangeActionWheel(1);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 102014 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 32);
    }

}