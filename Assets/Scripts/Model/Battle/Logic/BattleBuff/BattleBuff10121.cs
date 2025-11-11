using cfg;

public class BattleBuff10121 : BattleBuffBase
{
    private bool IsTrigger { get; set; }
    protected override float OnGetProperty(BattlePropertyType propertyType)
    {
        if (propertyType == BattlePropertyType.DefendInt)
        {
            return Config.ParamEx[0] + Config.ParamEx[1] * Subject.Gr;
        }

        return 0;
    }

    protected override void OnTriggerBuffMomentByCountIgnoreLayerCount(int count, MomentParamModel paramModel)
    {
        IsTrigger = true;
    }

    protected override void OnAfterUnderAction(MomentParamModel paramModel)
    {
        if (IsTrigger)
        {
            IsTrigger = false;
            ReduceLayerCount(1);
        }
    }
    

    public override void Recycle()
    {
        IsTrigger = false;
        base.Recycle();
    }
}
