using cfg;

public class BattleBuff90018 : BattleBuffBase
{
    private bool IsTrigger { get; set; }

    public override void SelfActionWheelStart()
    {
        IsTrigger = true;
        base.SelfActionWheelStart();
    }

    protected override int OnGetChangeActionWheel()
    {
        if (IsTrigger)
        {
            return Config.ParamEx[0].ToInt();
        }

        return 0;
    }
    protected override void OnBuffRecycle()
    {
        IsTrigger = false;
    }
}
