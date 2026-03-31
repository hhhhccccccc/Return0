using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2002 : BattleSkillBase
{
    //刚炁+30
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 30, BattleSource.Skill);
    }
}