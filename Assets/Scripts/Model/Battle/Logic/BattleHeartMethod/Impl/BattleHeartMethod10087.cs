
//效果给对面加了buff  对面一次性移除4层则会给施法者加一次行动次数
public class BattleHeartMethod10087 : BattleHeartMethodBase
{
    public override void BattleStart()
    {
        base.BattleStart();
        var oppoList = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
        foreach (var unit in oppoList)
        {
            BattleBuffManager.AddBuff(unit, GameConst.Battle.Buff90020, Subject, 1);
        }
    }
}