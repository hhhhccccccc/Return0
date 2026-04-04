public class BattleHeartMethod10114 : BattleHeartMethodBase
{
    private int Times => GetConfigParamInt(0);
    private bool CanTrigger { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        Register<UnitTriggerEndActionEventModel>(OnUnitTriggerEndAction);
    }

    private void OnUnitTriggerEndAction(UnitTriggerEndActionEventModel model)
    {
        if (!CanTrigger)
        {
            return;
        }

        if (Subject.ActionTimes == 0)
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