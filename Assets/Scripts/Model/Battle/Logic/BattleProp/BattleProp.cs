public class BattleProp : IModel, IRecycle
{
    public int ItemID { get; set; }
    public int Count { get; set; }
    
    public void Recycle()
    {
        ItemID = 0;
        Count = 0;
    }
}
