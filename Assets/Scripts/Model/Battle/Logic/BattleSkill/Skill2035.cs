using System.Collections.Generic;
using Zenject;

public class Skill2035 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122012103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20121, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102003 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 15);
    }

}