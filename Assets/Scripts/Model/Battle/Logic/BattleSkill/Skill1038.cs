using System.Collections.Generic;
using Zenject;

public class Skill1038 : BattleSkillBase
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
        // 效果: 4401202 - ClearBuffByType
        DoClearBuffByType(Subject, 2, 2);
        // 效果: 111017110 - AddBuff
        DoAddBuff(Subject, 10171, Subject, 10, null, BattleMomentType.ReleaseSkillAction);
    }

}