using cfg;
using Zenject;

public abstract class BattleMomentCondition : IModel
{
    [Inject] protected IConfigManager ConfigManager;
    protected BattleUnit Subject;
    protected BattleUnit Target;
    protected BattleUnit Spellcaster;
    protected BattleMomentConditionConfig Config;

    public bool Condition(int conditionID, BattleUnit subject, BattleUnit target)
    {
        Subject = subject;
        Target = target;
        Config = ConfigManager.GetBattleMomentCondition(conditionID);
        return OnCondition();
    }
    
    public bool Condition(int conditionID, BattleUnit subject, BattleUnit target, BattleUnit spellcaster)
    {
        Subject = subject;
        Target = target;
        Spellcaster = spellcaster;
        Config = ConfigManager.GetBattleMomentCondition(conditionID);
        return OnCondition();
    }

    protected abstract bool OnCondition();
}