using cfg;
using Zenject;

public abstract class BattleMomentCondition : IModel
{
    [Inject] protected ConfigManager ConfigManager;
    [Inject] protected BattleManager BattleManager;
    protected BattleUnit Subject;
    protected BattleUnit Target;
    protected BattleUnit Spellcaster;
    protected BattleUnit ClashTarget;
    protected BattleMomentConditionConfig Config;
    protected MomentParamModel ParamModel;
    protected int SkillID;
    protected int BuffLayerCount;
    public bool Condition(int conditionID, BattleUnit subject, BattleUnit target, MomentParamModel paramModel)
    {
        Subject = subject;
        Target = target;
        Spellcaster = null;
        InitClashTarget();
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
        InitClashTarget();
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
    
    private void InitClashTarget()
    {
        if (ParamModel is DamageParamModel model)
        {
            if (model.AttackID == Subject.EntityID)
            {
                ClashTarget = BattleManager.GetUnit(model.HitID);
            }
            else if (model.HitID == Subject.EntityID)
            {
                ClashTarget = BattleManager.GetUnit(model.AttackID);
            }
            else
            {
                ClashTarget = null;
            }
        }

        ClashTarget = null;
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