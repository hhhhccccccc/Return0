using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1050 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900013 - ChangeActionWheel
        Subject.ChangeActionWheel(-3);
    }

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

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 400003 - AddRandomKey
        Subject.AddRandomKey(3, (ChangeKeyReason)4);
    }

}