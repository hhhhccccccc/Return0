public class BattleBehaviour : IModel, IRecycle
{
    public int SubjectID { get; set; }
    public int TargetID { get; set; }
    public BattleBehaviourType BehaviourType { get; set; }
    public int SkillID { get; set; }
    public bool NeedCostResource { get; set; }
    public bool IsRepeat { get; set; }
    public void Recycle()
    {
        SubjectID = 0;
        TargetID = 0;
        BehaviourType = BattleBehaviourType.None;
        SkillID = 0;
        NeedCostResource = false;
        IsRepeat = false;
    }
}
