using System.Collections.Generic;
using Zenject;

public class Skill4009 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 119001402 - AddBuff
        DoAddBuff(Subject, 90014, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 101001 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 15);
    }

}