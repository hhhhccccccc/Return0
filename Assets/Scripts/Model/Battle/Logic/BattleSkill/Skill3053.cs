using System.Collections.Generic;
using Zenject;

public class Skill3053 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122008101 - AddBuff
        if (Target != null) DoAddBuff(Target, 20081, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102012 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 25);
    }

}