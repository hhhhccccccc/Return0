//todo 表现
public class BattleHeartMethod10068 : BattleHeartMethodBase
{
    public override void BattleStart()
    {
        base.BattleStart();
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffFuXiaoJian, Subject, GetParamInt(0));
    }
}