using cfg;
using Zenject;

public class BattleTreasureBase : BattleTreasureMoment, IModel
{
    public int TreasureID;

    public BattleUnit Subject;
    
    public TreasureConfig Config;

    [Inject] private ConfigManager ConfigManager;

    public void Init(int treasureID, BattleUnit subject)
    {
        TreasureID = treasureID;
        Subject = subject;
        Config = ConfigManager.GetTreasureConfig(treasureID);
        InitMoment(this);
    }
}
