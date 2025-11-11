using cfg;
using Zenject;

public class BattleTreasureBase : BattleTreasureMoment, IModel, IBattlePropertyChanged
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
