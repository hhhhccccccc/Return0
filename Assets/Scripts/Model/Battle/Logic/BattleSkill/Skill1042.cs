using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1042 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 4;
    }
    
    //玄炁上限+55
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.MaxXuanQiInt, 55, BattleSource.Skill);
    }

    //玄炁+55
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 55, BattleSource.Skill);
    }
}