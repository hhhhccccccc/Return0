using System.Collections.Generic;
using Zenject;

public class Skill2043 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2400003 - ChangeSkillXuanQiCostByUnitRes
        Subject.GetSkill()?.SetXuanQiCost(Math.Min(Subject.GetProperty(BattlePropertyType.XuanQi) * 0.7, 70));
        // 效果: 112001102 - AddBuff
        DoAddBuff(Subject, 20011, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 142009102 - AddBuff
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                DoAddBuff(otherUnit, 20091, Subject, 2, null, BattleMomentType.BeforeClash);
            }
        }
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 122009102 - AddBuff
        if (Target != null) DoAddBuff(Target, 20091, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 4800001 - HealGangQiPctByCurr
        var currGangQi = Subject.GetProperty(BattlePropertyType.GangQi); Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, Math.Max((int)(currGangQi * 0.3), 9));
    }

}