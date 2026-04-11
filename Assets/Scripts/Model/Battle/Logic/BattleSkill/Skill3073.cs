using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3073 : BattleSkillBase
{
    //玄炁+30
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 30, BattleSource.Skill);
    }
}