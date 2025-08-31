using cfg;
using Zenject;

public class BattleHeartMethodBase : BattleHeartMethodMoment, IModel
{
    [Inject] private IConfigManager ConfigManager;
    private int HeartMethodID;
    public HeartMethodConfig Config;
    public BattleUnit Subject;
    
    public void Init(int heartMethodID, BattleUnit subject)
    {
        HeartMethodID = heartMethodID;
        Config = ConfigManager.GetHeartMethod(HeartMethodID);
        Subject = subject;
        InitMoment(this);
    }
}

