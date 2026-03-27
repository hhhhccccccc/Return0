using System.Collections.Generic;
using Zenject;

public class Skill3061 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2300004 - ChangeSkillGangQiCostByUnitRes
        Subject.GetSkill()?.SetGangQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.GangQi) * 0.7, 70));
        // 效果: 2900012 - ChangeActionWheel
        Subject.ChangeActionWheel(-2);
    }

}