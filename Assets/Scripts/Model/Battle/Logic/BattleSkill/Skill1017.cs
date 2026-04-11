using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1017 : BattleSkillBase
{
    //获得2层玄聚状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffXuanJu, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }

    //消耗全部的键获得（等量+4）随机的键，获得1次行动次数
    protected override void OnAfterAction(MomentParamModel paramModel)
    {
        var removeList = DoRemoveAllKey(Subject, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
        var addCount = removeList.Count + 4;
        DoAddRandomKey(Subject, addCount, ChangeKeyReason.SkillEffect);
        DoAddActionTimes(Subject, 1);
    }
}