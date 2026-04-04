using System.Collections.Generic;
using cfg;
using System.Linq;
using Zenject;

public class Skill4073 : BattleSkillBase
{
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        DoAddBuff(Target, 740731, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Subject, 740732, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}