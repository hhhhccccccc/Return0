using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10014 : BattleHeartMethodBase
{
    private int Times => GetConfigParamInt(1);
    public override void RoundStart()
    {
        if (BattleLogicStateManager.Round > GetConfigParamInt(0))
        {
            Subject.AddActionTimes(Times);
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddActionTimes, Times);
        }
    }
}