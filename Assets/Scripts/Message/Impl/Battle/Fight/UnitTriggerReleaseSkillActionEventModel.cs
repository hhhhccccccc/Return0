using cfg;

public class UnitTriggerReleaseSkillActionEventModel : MessageModel
{
    public int AttackerID { get; set; }
    public int HitID { get; set; }

    public override void Recycle()
    {
        AttackerID = 0;
        HitID = 0;
        base.Recycle();
    }
}
