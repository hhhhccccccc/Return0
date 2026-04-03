using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3047 : BattleSkillBase
{
    //施加1层盲目状态
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var clashUnit = GetClashUnit(paramModel);
        DoAddBuff(clashUnit, GameConst.Battle.BuffMangMu, Subject, 1, null, BattleMomentType.BeforeClash);
    }
}