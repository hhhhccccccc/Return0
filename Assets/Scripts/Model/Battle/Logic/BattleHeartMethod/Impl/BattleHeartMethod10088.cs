using System.Linq;

public class BattleHeartMethod10088 : BattleHeartMethodBase
{
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
            Subject.AddActionTimes(GetParamInt(0));
            CanTrigger = false;
        }
    }
    
    public override void RoundStart()
    {
        base.RoundStart();
        CanTrigger = true;
    }

    public override void Recycle()
    {
        CanTrigger = false;
        base.Recycle();
    }
}