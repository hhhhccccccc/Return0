using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1033 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111011102 - AddBuff
        DoAddBuff(Subject, 10111, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102008 - ChangeProperty
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 40);
        // 效果: 119000802 - AddBuff
        DoAddBuff(Subject, 90008, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}