using cfg;

public class GetPropertySourceModel : IModel, IRecycle
{
    public GetPropertySourceType SourceType { get; set; }
    public int ID { get; set; }
    public void Recycle()
    {
        SourceType = GetPropertySourceType.None;
        ID = 0;
    }
}
