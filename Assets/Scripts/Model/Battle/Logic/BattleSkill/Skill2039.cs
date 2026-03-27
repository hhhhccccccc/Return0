using System.Collections.Generic;
using Zenject;

public class Skill2039 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2400004 - ChangeSkillXuanQiCostByUnitRes
        Subject.GetSkill()?.SetXuanQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.XuanQi) * 0.4, 0));
        // 效果: 2900001 - ChangeActionWheel
        Subject.ChangeActionWheel(1);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 105001 - SetProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 50);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122010105 - AddBuff
        if (Target != null) DoAddBuff(Target, 20101, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 400002 - AddRandomKey
        Subject.AddRandomKey(2, (ChangeKeyReason)4);
    }

}