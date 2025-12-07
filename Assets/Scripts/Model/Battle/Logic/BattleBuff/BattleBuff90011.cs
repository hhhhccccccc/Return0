using cfg;

public class BattleBuff90011 : BattleBuffBase
{
    protected override float OnGetAddWellyRate(int skillGuid)
    {
        return Config.ParamEx[0];
    }
}
