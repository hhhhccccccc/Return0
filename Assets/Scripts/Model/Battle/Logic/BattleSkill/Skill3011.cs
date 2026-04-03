using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3011 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        return 1;
    }
    
    public override void DoDesitionAction(bool isPreDesition)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffHuanSu, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
    
    //todo 到下个回合前持续该行动，下个回合释放破竹
}