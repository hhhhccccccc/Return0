using System.Collections.Generic;
using Zenject;

public class Skill3004 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2300002 - ChangeSkillGangQiCostByUnitRes
        Subject.GetSkill()?.SetGangQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.GangQi) * 0.3, 30));
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 4900001 - HealXuanQiPctByCurr
        var currXuanQi = Subject.GetProperty(BattlePropertyType.XuanQi); Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, Math.Max((int)(currXuanQi * 0.3), 9));
    }

}