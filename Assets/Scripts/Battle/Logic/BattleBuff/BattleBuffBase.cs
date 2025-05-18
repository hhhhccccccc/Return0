using cfg;

public class BattleBuffBase : BattleBuffMoment, IModel
{
    public int BuffID;

    public BattleBuff Cfg;
    public void Init(int buffID)
    {
        BuffID = buffID;
        Cfg = ConfigManager.GetBuff(BuffID);
    }

    public void Start()
    {
        OnStart();
    }

    public virtual void OnStart()
    {
        
    }

    public void End()
    {
        OnEnd();
    }

    public virtual void OnEnd()
    {
        
    }
}
