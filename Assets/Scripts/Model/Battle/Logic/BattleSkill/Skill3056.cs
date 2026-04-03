using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3056 : BattleSkillBase
{
    //本回合未受到过2次直接伤害本次行动不会被破招
    protected override int DontBeCounterState(MomentParamModel paramModel)
    {
        if (CheckRoundBeDirectDamageTimes(Subject, 2, DataRelation.XiaoYu))
        {
            return 1;
        }
        
        return 0;
    }

    //施加1层晕眩状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffXuanYun, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}