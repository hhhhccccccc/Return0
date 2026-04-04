using System.Collections.Generic;
using Zenject;

public class Skill4051 : BattleSkillBase
{
    //本次行动加快1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoChangeActionWheel(Subject, 1);
    }
    
    //todo 直到下次行动前全部友方的力与技额外提升50+RG*5
}