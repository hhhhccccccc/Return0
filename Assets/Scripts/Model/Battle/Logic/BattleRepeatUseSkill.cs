public class BattleRepeatUseSkill : IModel, IRecycle
{
    public int TargetID { get; set; }
    public int SkillID { get; set; }
    public int RepeatCount { get; set; }
    public int MaxRepeatCount { get; set; }
    public bool IfLostChangeToOther { get; set; }
    public void Recycle()
    {
        TargetID = 0;
        SkillID = 0;
        RepeatCount = 0;
        MaxRepeatCount = 0;
        IfLostChangeToOther = false;
    }
}
