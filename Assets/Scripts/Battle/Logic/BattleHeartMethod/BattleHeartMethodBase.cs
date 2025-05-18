using cfg;

public class BattleHeartMethodBase : BattleHeartMethodMoment, IModel
{
    public int HeartMethodID;

    public HeartMethod Cfg;
    
    public void Init(int heartMethodID)
    {
        HeartMethodID = heartMethodID;
        Cfg = ConfigManager.GetHeartMethod(HeartMethodID);
    }
}

