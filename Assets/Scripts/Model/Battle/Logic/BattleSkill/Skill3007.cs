using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3007 : BattleSkillBase
{
    //本次行动加快2息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, 2);
    }

    //玄炁+15
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 15, BattleSource.Skill);
    }
}