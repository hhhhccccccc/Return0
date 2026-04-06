using cfg;

public class BattleHeartMethod10071 : BattleHeartMethodBase
{
    private bool CanTrigger { get; set; }

    public override void AfterClash(MomentParamModel paramModel)
    {
        if (CanTrigger)
        {
            return;
        }
        if (paramModel is DamageParamModel model)
        {
            if (model.GetSelfClashState(Subject.EntityID))
            {
                CanTrigger = true;
            }
        }
    }

    public override void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        if (pType == BattlePropertyType.GangQi || pType == BattlePropertyType.XuanQi)
        {
            value += Util.GetRandomInt(GetConfigParamInt(0), GetConfigParamInt(1));
            CanTrigger = false;
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        CanTrigger = false;
    }
}