using cfg;

public class BattleTreasure10070 : BattleTreasureBase
{
    private bool CanTrigger { get; set; }
    private bool InTrigger { get; set; } 
    private int CD { get; set; }
    public override void Init(int treasureID, BattleUnit subject)
    {
        base.Init(treasureID, subject);
        CanTrigger = true;
        InTrigger = false;
        CD = 0;
    }

    protected override void OnRoundStart()
    {
        if (CanTrigger)
        {
            InTrigger = true;
            Subject.AddRandomKey(GetParamInt(1), ChangeKeyReason.TreasureEffect);
            CanTrigger = false;
            CD = GetParamInt(2);
        }
    }

    protected override void OnRoundEnd()
    {
        if (CD > 0 && !CanTrigger)
        {
            CD--;
            if (CD <= 0)
            {
                CanTrigger = true;
            }
        }

        InTrigger = false;
    }

    public override float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (!InTrigger)
        {
            return 0;
        }

        if (propertyType == BattlePropertyType.SpeedInt)
        {
            return GetParamFloat(0);
        }

        return 0;
    }
    
    protected override void OnRecycle()
    {
        CanTrigger = false;
        InTrigger = false;
        CD = 0;
    }
}
