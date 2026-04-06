using cfg;

public class BattleHeartMethod10058 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffNiSha, Subject, GetConfigParamInt(0), null, BattleMomentType.RoundStart);
    }
}