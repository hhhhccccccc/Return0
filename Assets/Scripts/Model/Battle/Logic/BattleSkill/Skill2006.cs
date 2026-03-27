using System.Collections.Generic;
using Zenject;

public class Skill2006 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2400003 - ChangeSkillXuanQiCostByUnitRes
        Subject.GetSkill()?.SetXuanQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.XuanQi) * 0.7, 70));
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 4800003 - HealGangQiPctByCurr
        var currGangQi = Subject.GetProperty(BattlePropertyType.GangQi); Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, Math.Max((int)(currGangQi * 0.7), 21));
    }

}