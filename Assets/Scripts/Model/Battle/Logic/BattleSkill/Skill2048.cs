using System.Collections.Generic;
using Zenject;

public class Skill2048 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 111001103 - AddBuff
        DoAddBuff(Subject, 10011, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 101008 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 10);
    }

}