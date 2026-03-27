using System.Collections.Generic;
using Zenject;

public class Skill2004 : BattleSkillBase
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
        // 效果: 4800001 - HealGangQiPctByCurr
        var currGangQi = Subject.GetProperty(BattlePropertyType.GangQi); Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, Math.Max((int)(currGangQi * 0.3), 9));
    }

}