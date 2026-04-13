using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3054 : BattleSkillBase
{
    //玄炁+35
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 35, BattleSource.Skill);
    }
}