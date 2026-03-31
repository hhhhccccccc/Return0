using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2031 : BattleSkillBase
{
    //若处于第一息则施加1层僵硬状态和2层破绽状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (BattleLogicStateManager.ActionWheel == 1)
        {
            DoAddBuff(Target, GameConst.Battle.BuffJiangYing, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
            DoAddBuff(Target, GameConst.Battle.BuffPoZhan, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        }
    }
}