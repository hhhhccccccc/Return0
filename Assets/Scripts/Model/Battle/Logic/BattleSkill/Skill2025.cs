using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2025 : BattleSkillBase
{
    //本次行动不会被未带有↓类留劲状态的敌手破招
    protected override int DontBeCounter(MomentParamModel paramModel)
    {
        return 3;
    }
    
    //施加3层刚屏合3层玄屏
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffGangPing, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffXuanPing, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
}