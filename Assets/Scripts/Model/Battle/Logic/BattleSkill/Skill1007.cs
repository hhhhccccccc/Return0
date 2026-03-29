using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill1007 : BattleSkillBase
{
    //行动加快2息
    public override void DoDesitionAction(bool isPreDesition)
    {
        base.DoDesitionAction(isPreDesition);
        DoChangeActionWheel(Subject, 2);
    }

    //指定友方获得2层迅速，清除目标的缓速状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        base.ReleaseSkillAction(paramModel);
        DoAddBuff(Target, GameConst.Battle.BuffXunSu, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
        DoRemoveBuff(Subject, GameConst.Battle.BuffHuanSu, 0);
    }

    //玄炁+20，双方获得2个随机的键
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        DoChangeProperty(Subject, BattlePropertyType.XuanQi, 20, BattleSource.Skill);
        DoAddRandomKey(Subject, 2, ChangeKeyReason.SkillEffect);
        DoAddRandomKey(Target, 2, ChangeKeyReason.SkillEffect);
    }
}