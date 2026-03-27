using System.Collections.Generic;
using Zenject;

public class Skill2045 : BattleSkillBase
{
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 119001101 - AddBuff
        DoAddBuff(Subject, 90011, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 101005 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 20);
    }

}