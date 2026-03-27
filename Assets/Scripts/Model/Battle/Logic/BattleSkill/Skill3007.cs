using System.Collections.Generic;
using Zenject;

public class Skill3007 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900002 - ChangeActionWheel
        Subject.ChangeActionWheel(2);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102003 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 15);
    }

}