using System.Collections.Generic;
using Zenject;

public class Skill2007 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122019103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20191, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 101008 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 10);
    }

}