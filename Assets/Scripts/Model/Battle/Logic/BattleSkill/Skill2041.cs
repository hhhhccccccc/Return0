using System.Collections.Generic;
using Zenject;

public class Skill2041 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2400005 - ChangeSkillXuanQiCostByUnitRes
        Subject.GetSkill()?.SetXuanQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.XuanQi) * 0.6, 60));
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122031103 - AddBuff
        if (Target != null) DoAddBuff(Target, 20311, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 4800001 - HealGangQiPctByCurr
        var currGangQi = Subject.GetProperty(BattlePropertyType.GangQi); Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, Math.Max((int)(currGangQi * 0.3), 9));
    }

}