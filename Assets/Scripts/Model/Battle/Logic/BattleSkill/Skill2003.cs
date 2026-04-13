using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2003 : BattleSkillBase
{
    //刚炁+90
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.GangQi, 90, BattleSource.Skill);
    }
}