using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3067 : BattleSkillBase
{
    //获得2层急速状态
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffXunSu, Subject, 2, null, BattleMomentType.AfterAction);
    }
}