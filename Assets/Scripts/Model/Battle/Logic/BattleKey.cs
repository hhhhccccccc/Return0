using cfg;

public class BattleKey : IModel, IRecycle
{
    private static int Guid = 0;
    public int KeyGuid { get; set; } = 0;
    public BattleKeyType KeyType { get; set; } = BattleKeyType.None; 
    public bool Locked { get; set; } = false;

    public void AllocGuid()
    {
        Guid++;
        KeyGuid = Guid;
    }
    
    public void Recycle()
    {
        KeyGuid = 0;
        KeyType = BattleKeyType.None;
        Locked = false;
    }
}
