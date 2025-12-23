using cfg;

public class UnitChangeKeyEventModel : MessageModel
{
    public int UnitID { get; set; }
    public BattleKeyType KeyType { get; set; }
    public int Count { get; set; }
    public ChangeKeyReason Reason { get; set; }
    public ChangeKeyType ChangeType { get; set; }
    public override void Recycle()
    {
        UnitID = 0;
        KeyType = BattleKeyType.None;
        Count = 0;
        Reason = ChangeKeyReason.None;
        ChangeType = ChangeKeyType.None;
        base.Recycle();
    }
}
