
//todo 表现
public class BattleHeartMethod10078 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        base.RoundStart();
        var opponentList = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
        foreach (var target in opponentList)
        {
            BattleBuffManager.AddBuff(target, GameConst.Battle.BuffYaoDu, Subject, GetParamInt(0));
        }
    }
}