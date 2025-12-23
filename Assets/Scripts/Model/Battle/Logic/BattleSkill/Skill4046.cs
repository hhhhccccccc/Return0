using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill4046 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        BattleBuffManager.AddBuff(Subject, 30331, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
} 