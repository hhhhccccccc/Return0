using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3110 : BattleSkillBase
{
    //造成的伤害减少30%
    public override float AddDamagePct(MomentParamModel paramModel)
    {
        return -0.3f;
    }
    
    //施加2层过劲状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffGuoJin, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}