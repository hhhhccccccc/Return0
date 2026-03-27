using System.Collections.Generic;
using Zenject;

public class Skill4030 : BattleSkillBase
{
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2900012 - ChangeActionWheel
        Subject.ChangeActionWheel(-2);
        // 效果: 111012101 - AddBuff
        DoAddBuff(Subject, 10121, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
        // 效果: 111013101 - AddBuff
        DoAddBuff(Subject, 10131, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    public override void BeforeUnderAction(MomentParamModel paramModel)
    {
        base.BeforeUnderAction(paramModel);
        // 效果: 3000001 - SetActionWheelToNow
        // TODO: SetActionWheelToNow
    }

    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        // 效果: 142001104 - AddBuff
        if (paramModel is DamageParamModel dm)
        {
            var otherID = dm.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                DoAddBuff(otherUnit, 20011, Subject, 4, null, BattleMomentType.BeforeClash);
            }
        }
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 400002 - AddRandomKey
        Subject.AddRandomKey(2, (ChangeKeyReason)4);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 3400001 - AddActionTimes
        Subject.AddActionTimes(1);
    }

}