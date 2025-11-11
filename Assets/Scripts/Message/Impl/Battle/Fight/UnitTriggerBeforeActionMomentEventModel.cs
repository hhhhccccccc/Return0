using cfg;

public class UnitTriggerBeforeActionMomentEventModel : MessageModel
{
    public int AttackerID { get; set; }
    public int HitID { get; set; }
    public BattleClashType ClashType { get; set; }
    public override void Recycle()
    {
        AttackerID = 0;
        HitID = 0;
        ClashType = BattleClashType.None;
        base.Recycle();
    }
}
