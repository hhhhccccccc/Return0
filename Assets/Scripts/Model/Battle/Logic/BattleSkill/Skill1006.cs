using System;
using System.Collections.Generic;
using Zenject;

public class Skill1006 : BattleSkillBase
{
    // Moment: 1006001 → 无条件 → 招式的刚炁消耗转为当前50%，至多50
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        // 效果: 2300001 - ChangeSkillGangQiCostByUnitRes
        // ParamList: [1, 0.5, 50] → 自己，50%，至多50
        DoChangeSkillGangQiCost(Subject, 0.5f, 50);
    }

    // Moment: 1006002 → 无条件 → 自己获取80%力的护体
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 3800001 - GetShieldBuffByPowerPct
        // ParamList: [1, 0.8] → 自己，80%
        DoGetShieldBuff(Subject, 0.8f, BattleMomentType.ReleaseSkillAction);
    }

    // Moment: 1006003 → 无条件 → 获取100%力的甲
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 3810001 - GetArmorBuffByPowerPct
        // ParamList: [1] → 自己100%
        DoGetArmorBuff(Subject, 1.0f, BattleMomentType.AfterAction);
    }
}