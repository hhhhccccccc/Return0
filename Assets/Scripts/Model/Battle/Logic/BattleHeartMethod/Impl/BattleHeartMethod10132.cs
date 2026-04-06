using cfg;

public class BattleHeartMethod10132 : BattleHeartMethodBase
{
    private bool CanTrigger { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanTrigger = true;
    }

    public override void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        if (CanTrigger && pType == BattlePropertyType.XuanQi && source == BattleSource.Natural)
        {
            value += GetConfigParamFloat(0);
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = false;
    }
}