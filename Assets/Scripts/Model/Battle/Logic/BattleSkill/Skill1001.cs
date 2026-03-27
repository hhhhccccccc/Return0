using System.Collections.Generic;
using Zenject;

public class Skill1001 : BattleSkillBase
{
    // Moment: 1001003 → 条件: 500001 → 效果: 谁直接变到当前息
    public override void BeforeUnderAction(MomentParamModel paramModel)
    {
        base.BeforeUnderAction(paramModel);
        // 条件: 500001 → CheckBeActionInBeforeActionWheel
        // ParamList: [1, 2, 0] → 目标前2息被调用了，是否包含当前息（0不包含）
        if (CheckBeActionInBeforeActionWheel(Subject, 2, false))
        {
            // 效果: 3000001 - SetActionWheelToNow → 谁直接变到当前息
            DoSetActionWheelToNow(Subject);
        }
    }

    // Moment: 1001004 → 条件: 700041 → 效果: 交锋者招式获得的气减少100
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                // 条件: 700041 → CheckSkillKillingStyle → 判断交锋者是杀式
                if (CheckSkillKillingStyle(otherUnit, true))
                {
                    // 效果: 119000701 - AddBuff → 交锋者招式获得的气减少100
                    // ParamList: [1, 4, 90007, 1] → 施法者→目标，90007号Buff，1层
                    DoAddBuff(otherUnit, 90007, Subject, 1, null, BattleMomentType.BeforeClash);
                }
            }
        }
    }

    // Moment: 1001006 → 无条件 → 我获得1个键
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 400001 - AddRandomKey → 我获得1个键
        DoAddRandomKey(Subject, 1, ChangeKeyReason.SkillEffect);
    }
}