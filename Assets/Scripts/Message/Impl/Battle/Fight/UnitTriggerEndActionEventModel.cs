using cfg;

public class UnitTriggerEndActionEventModel : MessageModel
{
    public int EntityID { get; set; }
    public override void Recycle()
    {
        EntityID = 0;
        base.Recycle();
    }
}
