using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10016 : BattleHeartMethodBase
{
    private int Times => GetParamInt(1);
    private bool CanTrigger { get; set; }
    public override void RoundStart()
    {
        base.RoundStart();
        if (CanTrigger)
        {
            CanTrigger = false;
            Subject.AddActionTimes(Times);
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddActionTimes, Times);
        }
    }

    public override void RoundEnd()
    {
        base.RoundEnd();
        if (BattleLogicStateManager.HasRoundUnitDie())
        {
            CanTrigger = true;
        }
    }

    protected override void OnRecycle()
    {
        CanTrigger = false;
    }
}