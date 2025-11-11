using cfg;
using Zenject;

public class BattleHeartMethodBase : BattleHeartMethodMoment, IModel, IBattlePropertyChanged
{
    [Inject] private ConfigManager ConfigManager;
    private int HeartMethodID;
    public HeartMethodConfig Config;
    public BattleUnit Subject;
    
    public void Init(int heartMethodID, BattleUnit subject)
    {
        HeartMethodID = heartMethodID;
        Config = ConfigManager.GetHeartMethodConfig(HeartMethodID);
        Subject = subject;
        InitMoment(this);
    }

    #region 战斗改变属性机制

    public float GetAddWellyRate(int skillGuid)
    {
        return 0;
    }

    public float GetAddWellyEffect(int skillGuid)
    {
        return 0;
    }

    public void TrySetBaseWellyRate(int skillGuid, ref float value)
    {
        
    }

    public void TrySetAddWellyRate(int skillGuid, ref float value)
    {
        
    }

    #endregion
}

