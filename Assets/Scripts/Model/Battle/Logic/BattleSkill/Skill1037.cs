using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1037 : BattleSkillBase
{
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 119000701 - AddBuff
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                DoAddBuff(otherUnit, 90007, Subject, 1, null, BattleMomentType.BeforeClash);
            }
        }
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 101006 - ChangeProperty
        Subject.ChangeProperty_Abs(BattlePropertyType.GangQi, 100);
        // 效果: 400005 - AddRandomKey
        Subject.AddRandomKey(5, (ChangeKeyReason)4);
    }

}