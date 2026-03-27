using System.Collections.Generic;
using Zenject;

public class Skill1012 : BattleSkillBase
{
    // Moment: 1012001 → 无条件 → 获取100%力的甲 + 恢复玄气5
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 3810001 - GetArmorBuffByPowerPct → 获取100%力的甲
        DoGetArmorBuff(Subject, 1.0f, BattleMomentType.ReleaseSkillAction);
        
        // 效果: 102004 - ChangeProperty → 恢复玄气5
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 5);
    }

    // Moment: 1012002 → 条件: 1400001 → 效果: 3810002
    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        base.AfterUnderAction(paramModel);
        // 条件: 1400001 → CheckSkillTriggerMoment → 自己当前技能是否经过行动后
        if (CheckSkillTriggerMoment(BattleMomentType.AfterAction))
        {
            // 效果: 3810002 - ConvertDamageToArmorBuff → 自己获得等量伤害的甲
            // TODO: 需要在战斗伤害结算时触发，暂时无法直接实现
            // DoConvertDamageToArmorBuff(Subject);
        }
    }
}