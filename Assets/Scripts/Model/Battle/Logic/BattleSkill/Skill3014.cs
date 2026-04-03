using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3014 : BattleSkillBase
{
    //招式的刚炁消耗转为当前50%，至多50
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeSkillCostByUnitRes(Subject, BattlePropertyType.GangQi, 0.5f, 50);
    }
}