using System.Collections.Generic;
using Zenject;

public class Skill3035 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 113008101 - AddBuff
        DoAddBuff(Subject, 30081, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 142011102 - AddBuff
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                DoAddBuff(otherUnit, 20111, Subject, 2, null, BattleMomentType.BeforeClash);
            }
        }
    }

}