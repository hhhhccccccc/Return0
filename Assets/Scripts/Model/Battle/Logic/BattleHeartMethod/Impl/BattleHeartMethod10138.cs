//todo 表现
public class BattleHeartMethod10138 : BattleHeartMethodBase
{
    public override void BattleStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffYueHuanJiaShi, Subject, GetConfigParamInt(0));
    }

    public override void RoundStart()
    {
        if (BattleLogicStateManager.Round == GetConfigParamInt(1))
        {
            BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffYueHuanJiaShi, Subject, GetConfigParamInt(2));
        }
    }
}