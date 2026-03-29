using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1041 : BattleSkillBase
{
    protected override int ActionDontBeCounter()
    {
        return 4;
    }
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        // 效果: 玄炁上限+40
        DoChangeProperty(Subject, BattlePropertyType.MaxXuanQiInt, 40);
    }

    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        // 效果: 玄炁+40
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 40);
    }
}