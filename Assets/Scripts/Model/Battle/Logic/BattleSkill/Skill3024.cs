using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3024 : BattleSkillBase
{
    //玄炁+10
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 10, BattleSource.Skill);
    }
}