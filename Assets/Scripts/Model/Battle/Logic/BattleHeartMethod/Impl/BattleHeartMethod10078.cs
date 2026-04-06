
using cfg;

public class BattleHeartMethod10078 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        var opponentList = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
        foreach (var target in opponentList)
        {
            DoAddBuff(target, GameConst.Battle.BuffYaoDu, Subject, GetConfigParamInt(0), null, BattleMomentType.RoundStart);
        }
    }
}