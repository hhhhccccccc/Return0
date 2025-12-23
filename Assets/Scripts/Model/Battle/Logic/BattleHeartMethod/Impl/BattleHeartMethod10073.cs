using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10073 : BattleHeartMethodBase
{
    private int ActionWheel { get; set; }

    public override void RoundStart()
    {
        base.RoundStart();
        ActionWheel = Util.GetRandomInt(GetParamInt(0), GetParamInt(1) + 1);
    }

    public override bool CanIgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        var now = BattleLogicStateManager.ActionWheel;
        return now != ActionWheel;
    }

    public override void Recycle()
    {
        ActionWheel = 0;
        base.Recycle();
    }
}