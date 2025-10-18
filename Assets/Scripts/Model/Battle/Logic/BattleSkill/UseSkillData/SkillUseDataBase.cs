using System.Collections.Generic;

public class SkillUseDataBase : IModel, IRecycle
{
    public int SkillID { get; set; }
    public int Round { get; set; }
    public int EndActionWheel { get; set; }
    public List<bool> ClashStateList = new();
    public void Recycle()
    {
        SkillID = 0;
        Round = 0;
        EndActionWheel = 0;
        ClashStateList.Clear();
    }
}
