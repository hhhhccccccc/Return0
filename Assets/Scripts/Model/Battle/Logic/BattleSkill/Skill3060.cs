using System.Collections.Generic;
using Zenject;

public class Skill3060 : BattleSkillBase
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
        // 效果: 102001 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 10);
        // 效果: 6200001 - TriggerBuffTimes
        // TODO: TriggerBuffTimes
    }

}