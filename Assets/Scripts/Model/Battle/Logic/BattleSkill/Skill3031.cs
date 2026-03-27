using System.Collections.Generic;
using Zenject;

public class Skill3031 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2300006 - ChangeSkillGangQiCostByUnitRes
        Subject.GetSkill()?.SetGangQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.GangQi) * 0.8, 80));
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 112011104 - AddBuff
        DoAddBuff(Subject, 20111, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
    }

}