using System.Collections.Generic;
using Zenject;

public class Skill4015 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 102010 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 30);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 101001 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 15);
    }

}