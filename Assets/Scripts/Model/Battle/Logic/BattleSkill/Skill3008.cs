using System.Collections.Generic;
using Zenject;

public class Skill3008 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2300005 - ChangeSkillGangQiCostByUnitRes
        Subject.GetSkill()?.SetGangQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.GangQi) * 0.6, 60));
        // 效果: 119001702 - AddBuff
        DoAddBuff(Subject, 90017, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}