using System.Collections.Generic;
using Zenject;

public class Skill2054 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122030103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20301, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102016 - ChangeProperty
        Target.ChangeProperty_Abs(BattlePropertyType.XuanQi, -20);
    }

}