using cfg;
using Zenject;

public class BattleTreasureBase : BattleTreasureMoment, IModel
{
    public int TreasureID;

    public BattleUnit Subject;
    
    public TreasureConfig Cfg;

    [Inject] private IConfigManager ConfigManager;

    public void Init(int treasureID, BattleUnit subject)
    {
        TreasureID = treasureID;
        Subject = subject;
        Cfg = ConfigManager.GetTreasure(treasureID);
        InitMoment(this);
    }
}
