using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1027 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 6;
    }
    // Skill: 1027
    // XuanQiCost: 20, NeedKey: [2, 4], ActionDontBeCounter: 6
    // Moments: BeforeClashMoment [1027003, 1027004]
    // Condition: 1027003 has ConditionID [700041], 1027004 has ConditionID [1300001]
    
    // Moment: 1027003 → 条件: 700041 (判断交锋者是杀式) → 效果: 交锋者刚气-20
    // Moment: 1027004 → 条件: 1300001 (判断是否互为目标) → 效果: 自己刚气+20
    public override void BeforeClash(MomentParamModel paramModel)
    {
        base.BeforeClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            var otherID = model.GetOtherID(Subject.EntityID);
            var otherUnit = BattleManager.GetUnit(otherID);
            
            // 1027003: 条件700041 → 判断交锋者是杀式 → 交锋者刚气-20
            if (otherUnit != null && CheckSkillIsKillingStyle(otherUnit, true))
            {
                // 效果: 101018 - ChangeProperty → 交锋者刚气-20
                DoChangeProperty(otherUnit, BattlePropertyType.GangQi, -20);
            }
            
            // 1027004: 条件1300001 → 判断是否互为目标 → 自己刚气+20
            if (CheckMutualGoal(Subject, otherUnit))
            {
                // 效果: 101005 - ChangeProperty → 自己刚气+20
                DoChangeProperty(Subject, BattlePropertyType.GangQi, 20);
            }
        }
    }
}