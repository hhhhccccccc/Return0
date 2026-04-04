using cfg;

public class BattleTreasure10070 : BattleTreasureBase
{
    private bool CanTrigger => CD <= 0;
    private bool InTrigger { get; set; } 
    private int CD { get; set; }
    public override void Init(int treasureID, BattleUnit subject)
    {
        base.Init(treasureID, subject);
        InTrigger = false;
        CD = 0;
    }

    protected override void OnRoundStart()
    {
        if (CanTrigger)
        {
            InTrigger = true;
            CD = GetConfigParamInt(2);
            var addKeyList = Subject.AddRandomKey(GetConfigParamInt(1), ChangeKeyReason.TreasureEffect);
            if (addKeyList is { Count: > 0 })
            {
                var viewModel = AllocViewModel(Subject.EntityID, MomentViewType.AddKey, Subject.EntityID);
                viewModel.AddKeyList(addKeyList);
                EnqueueViewModel(viewModel);
            }
        }
    }

    protected override void OnRoundEnd()
    {
        if (CD > 0 && !CanTrigger)
        {
            CD--;
        }

        InTrigger = false;
    }

    protected override float OnGetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (!InTrigger)
        {
            return 0;
        }

        if (propertyType == BattlePropertyType.SpeedInt)
        {
            return GetConfigParamFloat(0);
        }

        return 0;
    }
    
    protected override void OnTreasureRecycle()
    {
        InTrigger = false;
        CD = 0;
    }
}
