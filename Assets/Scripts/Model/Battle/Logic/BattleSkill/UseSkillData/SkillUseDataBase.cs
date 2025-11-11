using System.Collections.Generic;

public class SkillUseDataBase : IModel, IRecycle
{
    public int Guid { get; set; }
    public int SkillID { get; set; }
    public int VariantID { get; set; }
    public int Round { get; set; }
    public int EndActionWheel { get; set; }
    public List<bool> ClashStateList = new();
    public void Recycle()
    {
        Guid = 0;
        SkillID = 0;
        VariantID = 0;
        Round = 0;
        EndActionWheel = 0;
        ClashStateList.Clear();
    }
}
