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
        if (now != ActionWheel)
        {
            EnqueueViewModel(Subject.EntityID, MomentViewType.HeartMethod10073);
            return true;
        }

        return false;
    }

    protected override void OnRecycle()
    {
        ActionWheel = 0;
    }
}