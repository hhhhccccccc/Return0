public class BattleTreasure10001 : BattleTreasureBase
{
    public override void Init(int treasureID, BattleUnit subject)
    {
        base.Init(treasureID, subject);
        subject.AddHeartMethod(GameConst.Battle.HeartMethod10058);
    }
}
