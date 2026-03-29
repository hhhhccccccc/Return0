using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1005 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 1;
    }
    // Moment: 1005003 → 条件: 700041 → 效果: 交锋者招式获得的气减少100
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            // 条件: 700041 → CheckSkillKillingStyle → 判断交锋者是杀式
            if (CheckSkillIsKillingStyle(otherUnit, true))
            {
                // 效果: 119000701 - AddBuff → 交锋者招式获得的气减少100
                DoAddBuff(otherUnit, 90007, Subject, 1, null, BattleMomentType.BeforeClash);
            }
        }
    }

    // Moment: 1005005 → 无条件 → 我获得1个键
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 400001 - AddRandomKey → 我获得1个键
        DoAddRandomKey(Subject, 1, ChangeKeyReason.SkillEffect);
    }
}