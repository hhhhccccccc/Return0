using System.Collections.Generic;
using Zenject;

public class Skill3059 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2400009 - ChangeSkillXuanQiCostByUnitRes
        Subject.GetSkill()?.SetXuanQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.XuanQi) * 0.8, 80));
    }

    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 111009101 - AddBuff
        DoAddBuff(Subject, 10091, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}