using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3003 : BattleSkillBase
{
    //玄炁+90
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 90, BattleSource.Skill);
    }
}