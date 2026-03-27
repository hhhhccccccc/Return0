using System.Collections.Generic;
using Zenject;

public class Skill4004 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2400002 - ChangeSkillXuanQiCostByUnitRes
        Subject.GetSkill()?.SetXuanQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.XuanQi) * 0.4, 40));
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 111012101 - AddBuff
        DoAddBuff(Subject, 10121, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}