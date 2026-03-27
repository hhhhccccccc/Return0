using System.Collections.Generic;
using Zenject;

public class Skill3032 : BattleSkillBase
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
        // 效果: 122013102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20131, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

}