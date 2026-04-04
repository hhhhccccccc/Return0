using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4020 : BattleSkillBase
{
    //todo 本次行动不影响状态的存续
    
    //获得1次行动次数
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddActionTimes(Subject, 1);
    }
    
    //获得6层稳势状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffWenShi, Subject, 6, null, BattleMomentType.ReleaseSkillAction);
    }
}