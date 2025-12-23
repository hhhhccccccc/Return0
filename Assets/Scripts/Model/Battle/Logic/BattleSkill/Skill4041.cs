using System;
using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill4041 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel); 
        BattleBuffManager.AddBuff(Subject, 30351, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}