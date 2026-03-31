using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1018 : BattleSkillBase
{
    protected override int DontBeCounter(MomentParamModel paramModel)
    {
        return 4;
    }
    
    //玄炁上限+40%
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.MaxXuanQiPct, 0.4f, BattleSource.Skill);
    }

    //玄炁+40%
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQiPct, 0.4f, BattleSource.Skill);
    }
}