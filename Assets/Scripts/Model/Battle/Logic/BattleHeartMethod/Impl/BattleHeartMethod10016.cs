using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10016 : BattleHeartMethodBase
{
    private bool CanTrigger { get; set; }

    public override void RoundStart()
    {
        base.RoundStart();
        if (CanTrigger)
        {
            Subject.AddActionTimes(GetParamInt(0));
            CanTrigger = false;
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

    public override void Recycle()
    {
        base.Recycle();
        CanTrigger = false;
    }
}