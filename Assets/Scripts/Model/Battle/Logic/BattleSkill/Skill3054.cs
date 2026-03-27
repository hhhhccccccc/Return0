using System.Collections.Generic;
using Zenject;

public class Skill3054 : BattleSkillBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102013 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 35);
    }

}