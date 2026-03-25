public class BattleHeartMethod10043 : BattleHeartMethodBase
{
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        subject.MaxPotionCount += GetParamInt(0);
    }
}