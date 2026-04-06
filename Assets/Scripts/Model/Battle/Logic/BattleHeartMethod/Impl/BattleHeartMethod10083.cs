using cfg;

public class BattleHeartMethod10083 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        var opponentList = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
        foreach (var target in opponentList)
        {
            var commonPool = ConfigHelper.RandomCommonPool(GetConfigParamInt(0));
            DoAddBuff(target, commonPool[0].ID, Subject, commonPool[0].Num, null, BattleMomentType.RoundStart);
        }
    }
}