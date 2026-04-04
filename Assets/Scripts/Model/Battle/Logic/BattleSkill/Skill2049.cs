using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill2049 : BattleSkillBase
{
    //施加3层玄屏状态
    public override void BeforeClash(MomentParamModel paramModel)
    { 
        var clashUnit = GetOtherUnit(paramModel);
        DoAddBuff(clashUnit, GameConst.Battle.BuffXuanPing, Subject, 3, null, BattleMomentType.BeforeClash);
    }
}