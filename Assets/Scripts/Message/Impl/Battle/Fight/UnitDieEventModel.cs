using cfg;

public class UnitDieEventModel : MessageModel
{
    public int DieID { get; set; }
    public override void Recycle()
    {
        DieID = 0;
        base.Recycle();
    }
}
