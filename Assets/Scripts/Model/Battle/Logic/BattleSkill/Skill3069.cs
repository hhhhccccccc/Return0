using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3069 : BattleSkillBase
{
    //施加1层失持状态
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetClashUnit(paramModel);
        DoAddBuff(clashUnit, GameConst.Battle.BuffShiChi, Subject, 1, null, BattleMomentType.BeforeClash);
    }

    //移除目标1个增益状态
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        DoClearBuffByType(Target, BuffType.Gain, 1);
    }
}