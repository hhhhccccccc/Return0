using cfg;

public class BattleKey : IModel
{
    private static int Guid;
    public int KeyGuid { get; set; }
    public BattleKeyType KeyType { get; set; } = BattleKeyType.None; 
    /// <summary>
    /// 是否被锁住
    /// </summary>
    public bool Locked { get; set; }
    /// <summary>
    /// 是否被污染
    /// </summary>
    public bool Pollution { get; set; }
    public void AllocGuid()
    {
        Guid++;
        KeyGuid = Guid;
    }

    public int GetPriority()
    {
        var priority = 0;
        if (Pollution)
        {
            priority--;
        }

        return priority;
    }
}
