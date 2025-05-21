using cfg;
using Zenject;

public class BattleTreasureBase : BattleTreasureMoment, IModel
{
    public int TreasureID;
    
    public Treasure Cfg;

    [Inject] private IConfigManager ConfigManager;
    public void Init(int treasureID)
    {
        TreasureID = treasureID;
        Cfg = ConfigManager.GetTreasure(treasureID);
    }
}
