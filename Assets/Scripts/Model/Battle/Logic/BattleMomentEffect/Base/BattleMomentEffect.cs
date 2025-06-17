using cfg;
using Zenject;

public abstract class BattleMomentEffect : IModel
{
    [Inject] protected IConfigManager ConfigManager;
    protected BattleUnit Subject;
    protected BattleUnit Target;
    protected BattleUnit Spellcaster;
    protected BattleMomentEffectConfig Config;

    public void Effect(int momentEffectID, BattleUnit subject, BattleUnit target)
    {
        Subject = subject;
        Target = target;
        Config = ConfigManager.GetBattleMomentEffect(momentEffectID);
        OnEffect();
    }
    
    public void Effect(int momentEffectID, BattleUnit subject, BattleUnit target, BattleUnit spellcaster)
    {
        Subject = subject;
        Target = target;
        Spellcaster = spellcaster;
        Config = ConfigManager.GetBattleMomentEffect(momentEffectID);
        OnEffect();
    }

    protected abstract void OnEffect();
}