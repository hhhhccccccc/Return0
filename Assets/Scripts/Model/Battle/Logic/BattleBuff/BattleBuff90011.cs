using cfg;

public class BattleBuff90011 : BattleBuffBase
{
    protected override float OnGetWellyRateEx(int skillGuid)
    {
        return GetConfigParamFloat(0);
    }
}
