using cfg;
using Zenject;

public class BattleHeartMethodBase : BattleHeartMethodMoment, IModel
{
    [Inject] private IConfigManager ConfigManager;
    private int HeartMethodID;
    public HeartMethodConfig Cfg;
    public BattleUnit Subject;
    
    public void Init(int heartMethodID, BattleUnit subject)
    {
        HeartMethodID = heartMethodID;
        Cfg = ConfigManager.GetHeartMethod(HeartMethodID);
        Subject = subject;
        InitMoment(this);
    }
}

