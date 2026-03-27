using System.Collections.Generic;
using Zenject;

public class Skill2036 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900001 - ChangeActionWheel
        Subject.ChangeActionWheel(1);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122001101 - AddBuff
        if (Target != null) DoAddBuff(Target, 20011, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 101008 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 10);
    }

}