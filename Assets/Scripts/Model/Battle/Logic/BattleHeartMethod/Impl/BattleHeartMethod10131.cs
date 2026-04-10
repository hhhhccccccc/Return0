using cfg;

public class BattleHeartMethod10131 : BattleHeartMethodBase
{
    private bool CanTrigger { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        CanTrigger = true;
    }

    public override float GetMomentProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (CanTrigger && propertyType == BattlePropertyType.GangQiRecNatural)
        {
            return GetConfigParamFloat(0);
        }

        return 0;
    }

    public override void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
        BattleSource source = BattleSource.None)
    {
        if (propType == BattlePropertyType.GangQi && source == BattleSource.Natural)
        {
            CanTrigger = false;
        }
    }
    
    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = false;
    }
}