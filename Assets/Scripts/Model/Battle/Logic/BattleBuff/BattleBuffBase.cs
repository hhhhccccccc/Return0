using cfg;
using Zenject;

public class BattleBuffBase : BattleBuffMoment, IModel
{
    [Inject] private IConfigManager ConfigManager;
    private int BuffID;

    public BattleBuffConfig Cfg;
    public BattleUnit Spellcaster;
    public BattleUnit Subject;
    public void Init(int buffID, BattleUnit spellcaster, BattleUnit subject)
    {
        BuffID = buffID;
        Cfg = ConfigManager.GetBattleBuff(BuffID);
        Spellcaster = spellcaster;
        Subject = subject;
        InitMoment(this);
    }

    public void Start()
    {
        OnStart();
    }

    protected virtual void OnStart()
    {
        
    }

    public void End()
    {
        OnEnd();
    }

    protected virtual void OnEnd()
    {
        
    }
}
