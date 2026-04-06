using cfg;

public class BattleHeartMethod10138 : BattleHeartMethodBase
{
    public override void BattleStart()
    {
        DoAddBuff(Subject, GameConst.Battle.BuffYueHuanJiaShi, Subject, GetConfigParamInt(0), null, BattleMomentType.BattleStart);
    }

    public override void RoundStart()
    {
        if (BattleLogicStateManager.Round == GetConfigParamInt(1))
        {
            DoAddBuff(Subject, GameConst.Battle.BuffYueHuanJiaShi, Subject, GetConfigParamInt(2), null, BattleMomentType.RoundStart);
        }
    }
}