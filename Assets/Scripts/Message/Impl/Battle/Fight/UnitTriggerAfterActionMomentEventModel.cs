using cfg;

public class UnitTriggerAfterActionMomentEventModel : MessageModel
{
    public int EntityID { get; set; }
    public int SkillID { get; set; }
    public bool UseSuccess { get; set; }
    public override void Recycle()
    {
        EntityID = 0;
        SkillID = 0;
        UseSuccess = false;
        base.Recycle();
    }
}
