using System.Collections.Generic;
using Zenject;

public class Skill3066 : BattleSkillBase
{
    public override void SelfActionWheelStart()
    {
        base.SelfActionWheelStart();
        // 效果: 112008104 - AddBuff
        DoAddBuff(Subject, 20081, Subject, 4, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 111009102 - AddBuff
        DoAddBuff(Subject, 10091, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 142008102 - AddBuff
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                DoAddBuff(otherUnit, 20081, Subject, 2, null, BattleMomentType.BeforeClash);
            }
        }
        // 效果: 142008102 - AddBuff
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                DoAddBuff(otherUnit, 20081, Subject, 2, null, BattleMomentType.BeforeClash);
            }
        }
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 102001 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.XuanQi, 10);
    }

}