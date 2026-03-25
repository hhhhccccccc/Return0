//todo 表现
public class BattleHeartMethod10138 : BattleHeartMethodBase
{
    public override void BattleStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10191, Subject, GetParamInt(0));
    }

    public override void RoundStart()
    {
        if (BattleLogicStateManager.Round == GetParamInt(1))
        {
            BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10191, Subject, GetParamInt(2));
        }
    }
}