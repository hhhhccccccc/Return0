using System.Collections.Generic;
using Zenject;

public class Skill3033 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122008102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20081, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102001 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 10);
    }

}