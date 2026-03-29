using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1023 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 1;
    }
    
    // Skill: 铁筋功 (1023)
    // NeedKey: [2, 2, 3, 4], BreakDefendAddRate: 1
    // Moments: BeforeClashMoment [1023003], AfterActionMoment [1023004]
    // ActionDontBeCounter: 1
    
    // Moment: 1023003 → 无条件 → 自己给交锋者添加力衰20111,3层
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            if (otherUnit != null)
            {
                // 效果: 142011103 - AddBuff → 自己给交锋者添加力衰20111,3层
                // ParamList: [1, 4, 20111, 3] → 施法者→目标，20111号Buff，3层
                DoAddBuff(otherUnit, 20111, Subject, 3, null, BattleMomentType.BeforeClash);
            }
        }
    }

    // Moment: 1023004 → 无条件 → 我获得1个键
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 400001 - AddRandomKey → 我获得1个键
        // ParamList: [1, 1, 4] → 施法者，1个键，上限4
        DoAddRandomKey(Subject, 1, ChangeKeyReason.SkillEffect);
    }
}