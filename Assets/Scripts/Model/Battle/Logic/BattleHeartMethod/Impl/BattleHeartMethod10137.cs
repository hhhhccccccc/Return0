//todo 表现
public class BattleHeartMethod10137 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10191, Subject, GetParamInt(0));
    }
}