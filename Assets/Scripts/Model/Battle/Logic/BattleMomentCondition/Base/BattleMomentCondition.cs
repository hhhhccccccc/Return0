using cfg;
using Zenject;

public abstract class BattleMomentCondition : IModel
{
    [Inject] protected ConfigManager ConfigManager;
    protected BattleUnit Subject;
    protected BattleUnit Target;
    protected BattleUnit Spellcaster;
    protected BattleMomentConditionConfig Config;
    protected MomentParamModel ParamModel;
    protected int SkillID;
    protected int BuffLayerCount;
    public bool Condition(int conditionID, BattleUnit subject, BattleUnit target, MomentParamModel paramModel)
    {
        Subject = subject;
        Target = target;
        Spellcaster = null;
        Config = ConfigManager.GetBattleMomentConditionConfig(conditionID);
        ParamModel = paramModel;
        SkillID = 0;
        BuffLayerCount = 0;
        return OnCondition();
    }
    
    public bool Condition(int conditionID, BattleUnit subject, BattleUnit target, BattleUnit spellcaster, MomentParamModel paramModel, int layerCount)
    {
        Subject = subject;
        Target = target;
        Spellcaster = spellcaster;
        Config = ConfigManager.GetBattleMomentConditionConfig(conditionID);
        ParamModel = paramModel;
        SkillID = 0;
        BuffLayerCount = layerCount;
        return OnCondition();
    }
    
    public bool Condition(int conditionID, BattleUnit subject, int skillID, MomentParamModel paramModel)
    {
        Subject = subject;
        Target = null;
        Spellcaster = null;
        SkillID = skillID;
        Config = ConfigManager.GetBattleMomentConditionConfig(conditionID);
        ParamModel = paramModel;
        BuffLayerCount = 0;
        return OnCondition();
    }

    protected abstract bool OnCondition();
    
    protected BattleUnit GetUnitByParamID(float paramID)
    {
        return paramID.ToInt() switch
        {
            1 => Subject,
            2 => Target,
            3 => Spellcaster,
            _ => null
        };
    }
}