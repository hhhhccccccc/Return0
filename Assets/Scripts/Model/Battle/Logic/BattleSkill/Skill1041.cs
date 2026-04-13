using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1041 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 4;
    }
    //玄炁上限+40
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.MaxXuanQiInt, 40, BattleSource.Skill);
    }

    //玄炁+40
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 40, BattleSource.Skill);
    }
}