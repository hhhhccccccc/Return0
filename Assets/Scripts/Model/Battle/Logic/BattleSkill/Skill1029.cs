using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1029 : BattleSkillBase
{
    //获得1层回避
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffHuiBi, Subject, 1, null, BattleMomentType.ReleaseSkillAction);
    }

    //获得4个不同的键
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 4, ChangeKeyReason.SkillEffect);
    }
}