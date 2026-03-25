using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10015 : BattleHeartMethodBase
{
    private int Times => GetParamInt(1);
    public override void RoundStart()
    {
        if (BattleLogicStateManager.Round <= GetParamInt(0))
        {
            Subject.AddActionTimes(Times);
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddActionTimes, Times);
        }
    }
}