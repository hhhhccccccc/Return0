using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2008 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 112001102 - AddBuff
        DoAddBuff(Subject, 20011, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 101007 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 5);
    }

}