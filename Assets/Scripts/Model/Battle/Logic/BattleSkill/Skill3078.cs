using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3078 : BattleSkillBase
{
    //施加2层盲目状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffMangMu, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}