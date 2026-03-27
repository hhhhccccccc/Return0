using System.Collections.Generic;
using Zenject;

public class Skill3077 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 101019 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 50);
    }

}