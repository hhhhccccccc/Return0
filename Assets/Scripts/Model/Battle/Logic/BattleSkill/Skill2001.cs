using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2001 : BattleSkillBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 101008 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 10);
    }

}