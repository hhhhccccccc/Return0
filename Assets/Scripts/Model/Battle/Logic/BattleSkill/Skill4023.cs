using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4023 : BattleSkillBase
{
    //双方随机获得2个键
    public override void BeforeClash(MomentParamModel paramModel)
    {
        DoAddRandomKey(Subject, 2, ChangeKeyReason.SkillEffect);
        var clashUnit = GetOtherUnit(paramModel);
        DoAddRandomKey(clashUnit, 2, ChangeKeyReason.SkillEffect);
    }

    //对目标施加3层失衡状态和3层伤口状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffShiHeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffShangKou, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }
}