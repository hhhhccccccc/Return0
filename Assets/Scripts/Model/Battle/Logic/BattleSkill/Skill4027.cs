using System.Collections.Generic;
using Zenject;

public class Skill4027 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 101005 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 20);
        // 效果: 102002 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 20);
        // 效果: 4401202 - ClearBuffByType
        DoClearBuffByType(Subject, 2, 2);
    }

}