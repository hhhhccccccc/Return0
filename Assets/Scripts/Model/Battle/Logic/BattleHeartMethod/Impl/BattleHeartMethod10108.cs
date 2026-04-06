using cfg;

public class BattleHeartMethod10108 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        if (Subject.GetProperty(BattlePropertyType.Hp) / Subject.GetProperty(BattlePropertyType.MaxHp) <=
            GetConfigParamFloat(0))
        {
            DoAddRandomKey(Subject, GetConfigParamInt(1), ChangeKeyReason.HeartMethodEffect);
        }
    }
}