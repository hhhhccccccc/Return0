using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4060 : BattleSkillBase
{
    //施加5层玄屏状态，减少其20防
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffXuanPing, Subject, 5, null, BattleMomentType.ReleaseSkillAction);
        DoChangeProperty(Target, BattlePropertyType.DefendInt, -20, BattleSource.Skill);
    }
}