using System.Collections.Generic;
using Zenject;

public class Skill4043 : BattleSkillBase
{
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 142012105 - AddBuff
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                DoAddBuff(otherUnit, 20121, Subject, 5, null, BattleMomentType.BeforeClash);
            }
        }
        // 效果: 142002101 - AddBuff
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                DoAddBuff(otherUnit, 20021, Subject, 1, null, BattleMomentType.BeforeClash);
            }
        }
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
        // 效果: 122012105 - AddBuff
        if (Target != null) DoAddBuff(Target, 20121, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 122002101 - AddBuff
        if (Target != null) DoAddBuff(Target, 20021, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

}