using System.Collections.Generic;
using Zenject;

public class Skill2003 : BattleSkillBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 101010 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 90);
    }

}