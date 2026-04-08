public class BattleSkillData : IModel, IRecycle
{
    public int Guid { get; set; }
    public int SkillID { get; set; }
    public int VariantID { get; set; }
    public void Recycle()
    {
        Guid = 0;
        SkillID = 0;
        VariantID = 0;
    }

    public virtual bool CheckSkillCanDoDesition(BattleUnit target)
    {
        return true;
    }
}
