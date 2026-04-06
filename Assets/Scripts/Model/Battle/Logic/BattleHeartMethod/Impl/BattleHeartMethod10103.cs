using cfg;

public class BattleHeartMethod10103 : BattleHeartMethodBase
{
    private int Times => GetConfigParamInt(1);
    public override void RoundStart()
    {
        base.RoundStart();
        if (Subject.GetProperty(BattlePropertyType.XuanQi) >= GetConfigParamFloat(0))
        {
            DoAddActionTimes(Subject, Times);
        }
    }
}