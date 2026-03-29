using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1019 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 4;
    }
    // Moment: 1019002 → 无条件 → 玄气百分比上限变化
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        //玄炁上限+55%
        DoChangeProperty(Subject, BattlePropertyType.MaxXuanQiPct, 0.55f);
    }

    // Moment: 1019003 → 无条件 → 玄气百分比变化
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        //玄炁+55%
        DoChangeProperty(Subject, BattlePropertyType.XuanQiPct, 0.55f);
    }
}