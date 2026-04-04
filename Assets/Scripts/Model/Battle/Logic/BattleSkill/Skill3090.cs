using System.Collections.Generic;
using cfg;
using Zenject;

public class Skill3090 : BattleSkillBase
{
    //双方毒瘴状态层数最高者获得2层毒瘴状态
    public override void BeforeClash(MomentParamModel paramModel)
    {
        var selfBuffCount = Subject.GetBuffCountByID(GameConst.Battle.BuffDuZhang);
        var clashUnit = GetOtherUnit(paramModel);
        var clashUnitBuffCount = clashUnit.GetBuffCountByID(GameConst.Battle.BuffDuZhang);
        if (selfBuffCount > clashUnitBuffCount)
        {
            DoAddBuff(Subject, GameConst.Battle.BuffDuZhang, Subject, 2, null, BattleMomentType.BeforeAction);
        }
        else if (selfBuffCount < clashUnitBuffCount)
        {
            DoAddBuff(clashUnit, GameConst.Battle.BuffDuZhang, Subject, 2, null, BattleMomentType.BeforeAction);
        }
    }

    //施加2层缓速
    public override void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        DoAddBuff(Target, GameConst.Battle.BuffHuanSu, Subject, 2, null, BattleMomentType.ReleaseSkillAction);
    }
}