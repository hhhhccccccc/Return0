public class SkillUseDataBase : IModel, IRecycle
{
    public int SkillID { get; set; }
    public int Round { get; set; }
    public int EndActionWheel { get; set; }
    public void Recycle()
    {
        SkillID = 0;
        Round = 0;
        EndActionWheel = 0;
    }
}
