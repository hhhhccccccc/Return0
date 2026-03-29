using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1018 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 4;
    }
    
    // Moment: 1018002 → 无条件 → 玄气百分比上限变化
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 玄炁上限+40%
        DoChangeProperty(Subject, BattlePropertyType.MaxXuanQiPct, 0.4f);
    }

    // Moment: 1018003 → 无条件 → 玄气百分比变化
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 玄炁+40%
        DoChangeProperty(Subject, BattlePropertyType.XuanQiPct, 0.4f);
    }
}