using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1008 : BattleSkillBase
{
    //行动加快1息
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        DoChangeActionWheel(Subject, 1);
    }

    //获得一层反击
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        DoAddBuff(Subject, GameConst.Battle.BuffFanJi, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    //todo 在3息内反击不会低于一层
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        DoAddBuff(Subject, GameConst.Battle.Buff90009, Subject, 4, null, BattleMomentType.AfterAction);
    }
}