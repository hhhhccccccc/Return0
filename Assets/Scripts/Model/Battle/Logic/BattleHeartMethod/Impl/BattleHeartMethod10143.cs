using cfg;

public class BattleHeartMethod10143 : BattleHeartMethodBase
{
    private int Times => GetParamInt(0);
    private float Accumulate { get; set; }
    private float Single { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        Single = subject.GetProperty(BattlePropertyType.BasicMaxHp) * GetParamFloat(0);
        Accumulate = 0;
    }

    public override void ReduceHp(float reduceHp, DamageType damageType, int attackID)
    {
        Accumulate += reduceHp;
        while (Accumulate >= Single)
        {
            Accumulate -= Single;
            Subject.AddActionTimes(Times);
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddActionTimes, Times);
        }
    }

    protected override void OnRecycle()
    {
        Accumulate = 0;
        Single = 0;
    }
}