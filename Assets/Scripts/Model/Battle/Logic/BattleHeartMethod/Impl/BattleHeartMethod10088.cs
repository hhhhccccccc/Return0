using System.Linq;

public class BattleHeartMethod10088 : BattleHeartMethodBase
{
    private int Times => GetParamInt(0);
    private bool CanTrigger { get; set; }
    
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanTrigger = true;
    }

    public override void BeCounter()
    {
        if (CanTrigger && Subject.Bf.GetAliveUnit().Any(o => o.EntityID != Subject.EntityID))
        {
            CanTrigger = false;
            Subject.AddActionTimes(Times);
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddActionTimes, Times);
        }
    }
    
    public override void RoundStart()
    {
        base.RoundStart();
        CanTrigger = true;
    }

    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = false;
    }
}