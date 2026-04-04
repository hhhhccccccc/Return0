using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4084 : BattleSkillBase
{
    //在本息将时段变为夜
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeChrono(ChronoType.Night, BattleChronoContinueType.ActionWheel, 1);
    }
}