using System.Collections.Generic;
using Zenject;

public class Skill4078 : BattleSkillBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 101019 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 50);
    }

}