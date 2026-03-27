using System.Collections.Generic;
using Zenject;

public class Skill4006 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 102010 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 30);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 111018104 - AddBuff
        DoAddBuff(Subject, 10181, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
    }

}