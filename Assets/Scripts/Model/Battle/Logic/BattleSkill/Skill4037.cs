using System.Collections.Generic;
using Zenject;

public class Skill4037 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900002 - ChangeActionWheel
        Subject.ChangeActionWheel(2);
    }

    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 142009103 - AddBuff
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                DoAddBuff(otherUnit, 20091, Subject, 3, null, BattleMomentType.BeforeClash);
            }
        }
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 102017 - ChangeProperty
        Target.ChangeProperty_Abs(BattlePropertyType.XuanQi, 35);
    }

}