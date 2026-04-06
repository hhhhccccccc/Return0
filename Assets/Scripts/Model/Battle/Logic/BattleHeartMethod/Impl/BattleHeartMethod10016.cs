using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10016 : BattleHeartMethodBase
{
    private int Times => GetConfigParamInt(1);
    private bool CanTrigger { get; set; }
    public override void RoundStart()
    {
        if (CanTrigger)
        {
            CanTrigger = false;
            DoAddActionTimes(Subject, Times);
        }
    }

    public override void RoundEnd()
    {
        if (BattleLogicStateManager.HasRoundUnitDie())
        {
            CanTrigger = true;
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = false;
    }
}