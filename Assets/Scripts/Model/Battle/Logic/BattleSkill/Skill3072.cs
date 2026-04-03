using System;
using System.Collections.Generic;
using cfg;
using System.Linq;

public class Skill3072 : BattleSkillBase
{
    //获得1层心眼状态和1层避殃状态
    public override void DoDesitionAction(bool isPreDesition)
    {
        DoAddBuff(Subject, GameConst.Battle.BuffXinYan, Subject, 1, null, BattleMomentType.DoDesitionAction);
        DoAddBuff(Subject, GameConst.Battle.BuffBiYang, Subject, 1, null, BattleMomentType.DoDesitionAction);
    }

    public override void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var gangQi = Target.GetProperty(BattlePropertyType.GangQi);
        var xuanQi = Target.GetProperty(BattlePropertyType.XuanQi);
        if (gangQi >= xuanQi)
        {
            var cost = gangQi * Config.ParamEx[0];
            cost = Math.Min(cost, Config.ParamEx[1]);
            DoChangeProperty(Target, BattlePropertyType.GangQi, -cost, BattleSource.Skill);
        }
        else
        {
            var cost = xuanQi * Config.ParamEx[0];
            cost = Math.Min(cost, Config.ParamEx[1]);
            DoChangeProperty(Target, BattlePropertyType.XuanQi, -cost, BattleSource.Skill);
        }
    }
}