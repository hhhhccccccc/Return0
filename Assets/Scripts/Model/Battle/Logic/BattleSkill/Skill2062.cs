using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2062 : BattleSkillBase
{
    protected override int DontBeCounterState(MomentParamModel paramModel)
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