using System.Collections.Generic;
using Zenject;

public class Skill3048 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2300004 - ChangeSkillGangQiCostByUnitRes
        Subject.GetSkill()?.SetGangQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.GangQi) * 0.7, 70));
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 112011103 - AddBuff
        DoAddBuff(Subject, 20111, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

}