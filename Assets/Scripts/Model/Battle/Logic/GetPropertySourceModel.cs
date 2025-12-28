using cfg;

public class GetPropertySourceModel : IModel, IRecycle
{
    public GetPropertySourceType SourceType { get; set; }
    public int TypeID { get; set; }
    public int AttackerID { get; set; }
    public int HitID { get; set; }
    public void Recycle()
    {
        SourceType = GetPropertySourceType.None;
        AttackerID = 0;
        HitID = 0;
        TypeID = 0;
    }
}
