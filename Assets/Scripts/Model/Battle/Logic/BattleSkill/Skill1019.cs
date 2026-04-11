using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1019 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 4;
    }
    //玄炁上限+55%
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.MaxXuanQiPct, 0.55f, BattleSource.Skill);
    }

    //玄炁+55%
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQiPct, 0.55f, BattleSource.Skill);
    }
}