using cfg;

public class BattleTreasureBase : BattleTreasureMoment, IModel
{
    public int TreasureID;
    
    public Treasure Cfg;
    
    public void Init(int treasureID)
    {
        TreasureID = treasureID;
        Cfg = ConfigManager.GetTreasure(treasureID);
    }
}
