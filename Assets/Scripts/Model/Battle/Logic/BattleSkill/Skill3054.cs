using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3054 : BattleSkillBase
{
    //玄炁+35
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 35, BattleSource.Skill);
    }
}