using cfg;
using Zenject;

public class BattleHeartMethodBase : BattleHeartMethodMoment, IModel
{
    public int HeartMethodID;

    public HeartMethod Cfg;
    [Inject] private IConfigManager ConfigManager;
    
    public void Init(int heartMethodID)
    {
        HeartMethodID = heartMethodID;
        Cfg = ConfigManager.GetHeartMethod(HeartMethodID);
    }
}

