using System.Collections.Generic;
using Zenject;

public class Skill4017 : BattleSkillBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 101016 - ChangeProperty
        Target.ChangeProperty_Abs(BattlePropertyType.GangQi, 10);
    }

}