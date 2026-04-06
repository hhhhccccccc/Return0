using cfg;

public class BattleBuff90018 : BattleBuffBase
{
    private bool IsTrigger { get; set; }

    protected override void OnSelfActionWheelStart()
    {
        IsTrigger = true;
    }
    
    protected override int OnGetChangeActionWheel()
    {
        if (IsTrigger)
        {
            return GetConfigParamInt(0);
        }

        return 0;
    }
    protected override void OnBuffRecycle()
    {
        IsTrigger = false;
    }
}
