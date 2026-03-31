using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2001 : BattleSkillBase
{
    //刚炁+10
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 10, BattleSource.Skill);
    }
}