
//todo 表现
public class BattleHeartMethod10083 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        base.RoundStart();
        var opponentList = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
        foreach (var target in opponentList)
        {
            var commonPool = ConfigHelper.RandomCommonPool(GetConfigParamInt(0));
            BattleBuffManager.AddBuff(target, commonPool[0].ID, Subject, commonPool[0].Num);
        }
    }
}