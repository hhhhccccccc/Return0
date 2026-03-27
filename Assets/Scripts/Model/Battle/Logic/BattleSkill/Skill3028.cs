using System.Collections.Generic;
using Zenject;

public class Skill3028 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2300001 - ChangeSkillGangQiCostByUnitRes
        Subject.GetSkill()?.SetGangQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.GangQi) * 0.5, 50));
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 200003 - ChangeHpByAttackDamage
        // TODO: ChangeHpByAttackDamage
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 4900004 - HealXuanQiPctByCurr
        var currXuanQi = Subject.GetProperty(BattlePropertyType.XuanQi); Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, Math.Max((int)(currXuanQi * 0.5), 15));
    }

}