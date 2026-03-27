using System.Collections.Generic;
using Zenject;

public class Skill4039 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 111016101 - AddBuff
        DoAddBuff(Subject, 10161, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 400002 - AddRandomKey
        Subject.AddRandomKey(2, (ChangeKeyReason)4);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 101011 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 25);
    }

}