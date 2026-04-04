using cfg;

public class BattleHeartMethod10143 : BattleHeartMethodBase
{
    private int Times => GetConfigParamInt(0);
    private float Accumulate { get; set; }
    private float Single { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        Single = subject.GetProperty(BattlePropertyType.BasicMaxHp) * GetConfigParamFloat(0);
        Accumulate = 0;
    }

    public override void AfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        if (!isReduce)
        {
            return;
        }
        
        Accumulate += changeHp;
        while (Accumulate >= Single)
        {
            Accumulate -= Single;
            Subject.AddActionTimes(Times);
            EnqueueViewModel(Subject.EntityID, MomentViewType.AddActionTimes, Times);
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        Accumulate = 0;
        Single = 0;
    }
}