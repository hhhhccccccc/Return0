using System.Collections.Generic;
using Zenject;

public class Skill3074 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2400001 - ChangeSkillXuanQiCostByUnitRes
        Subject.GetSkill()?.SetXuanQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.XuanQi) * 0.3, 30));
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 4900004 - HealXuanQiPctByCurr
        var currXuanQi = Subject.GetProperty(BattlePropertyType.XuanQi); Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, Math.Max((int)(currXuanQi * 0.5), 15));
    }

}