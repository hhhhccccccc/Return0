using System.Collections.Generic;
using Zenject;

public class Skill2046 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2400006 - ChangeSkillXuanQiCostByUnitRes
        Subject.GetSkill()?.SetXuanQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.XuanQi) * 0.5, 50));
    }

    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 101012 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, -10);
        // 效果: 101015 - ChangeProperty
        Target.ChangeProperty_Abs(BattlePropertyType.GangQi, -10);
    }

}