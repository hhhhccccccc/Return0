public class BattleTreasure10001 : BattleTreasureBase
{
    private int HeartMethodID => GameConst.Battle.HeartMethod10058;
    public override void Init(int treasureID, BattleUnit subject)
    {
        base.Init(treasureID, subject);
        subject.AddHeartMethod(HeartMethodID);
        EnqueueViewModel(Subject.EntityID, MomentViewType.AddHeartMethod, HeartMethodID);
    }
}
