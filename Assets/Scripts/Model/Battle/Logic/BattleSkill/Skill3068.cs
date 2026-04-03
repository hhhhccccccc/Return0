using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3068 : BattleSkillBase
{
    //目标未剩余行动次数则施加2层破绽
    public override void DoDesitionAction(bool isPreDesition)
    {
        if (Target.ActionTimes <= 0)
        {
            DoAddBuff(Target, GameConst.Battle.BuffPoZhan, Subject, 2, null, BattleMomentType.DoDesitionAction);
        }
    }

    //施加1层僵硬状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
         DoAddBuff(Target, GameConst.Battle.BuffJiangYing, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }
}