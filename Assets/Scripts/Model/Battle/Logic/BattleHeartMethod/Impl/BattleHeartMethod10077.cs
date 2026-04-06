using cfg;

public class BattleHeartMethod10077 : BattleHeartMethodBase
{
    public override void BattleStart()
    {
        var oppoList = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
        foreach (var unit in oppoList)
        {
            DoAddBuff(unit, GameConst.Battle.Buff90019, Subject, 1, null, BattleMomentType.BattleStart);
        }
    }
}