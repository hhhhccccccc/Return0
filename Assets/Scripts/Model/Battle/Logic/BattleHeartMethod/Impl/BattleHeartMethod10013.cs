using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleHeartMethod10013 : BattleHeartMethodBase
{
    private int Times => GetParamInt(1);
    public override void RoundStart()
    {
        base.RoundStart();
        if (Subject.GetProperty(BattlePropertyType.Hp) / Subject.GetProperty(BattlePropertyType.MaxHp) <= GetParamFloat(0))
        {
            Subject.AddActionTimes(Times);
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddActionTimes, Times);
        }
    }
}