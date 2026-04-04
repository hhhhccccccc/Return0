using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill4069 : BattleSkillBase
{
    //对目标施加5层失衡状态和5层伤口状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoAddBuff(Target, GameConst.Battle.BuffShiHeng, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
        DoAddBuff(Target, GameConst.Battle.BuffShangKou, Subject, 3, null, BattleMomentType.ReleaseSkillAction);
    }

    //消耗敌手2个随机的键
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetOtherUnit(paramModel);
        DoRemoveRandomKey(clashUnit, 2, ChangeKeyReason.SkillEffect, ChangeKeyType.Cost);
    }
}