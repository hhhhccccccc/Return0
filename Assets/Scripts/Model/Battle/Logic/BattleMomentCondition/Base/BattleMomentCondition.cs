using cfg;
using Zenject;

public abstract class BattleMomentCondition : IModel
{
    [Inject] protected ConfigManager ConfigManager  { get; set; }
    [Inject] protected BattleManager BattleManager  { get; set; }
    protected BattleUnit Subject  { get; set; }
    protected BattleUnit Target  { get; set; }
    protected BattleUnit SpellCaster  { get; set; }
    protected BattleUnit ActionTarget  { get; set; }
    protected BattleMomentConditionConfig Config  { get; set; }
    protected MomentParamModel ParamModel  { get; set; }
    protected int SkillID  { get; set; }
    protected int VariantID  { get; set; }
    protected int BuffLayerCount  { get; set; }
    public bool Condition(int conditionID, BattleUnit subject, BattleUnit target, MomentParamModel paramModel)
    {
        Subject = subject;
        Target = target;
        SpellCaster = null;
        InitActionTarget();
        SkillID = 0;
        VariantID = 0;
        Config = ConfigManager.GetBattleMomentConditionConfig(conditionID);
        ParamModel = paramModel;
        BuffLayerCount = 0;
        return OnCondition();
    }
    
    public bool Condition(int conditionID, BattleUnit subject, BattleUnit target, BattleUnit spellCaster, MomentParamModel paramModel, int layerCount)
    {
        Subject = subject;
        Target = target;
        SpellCaster = spellCaster;
        InitActionTarget();
        SkillID = 0;
        VariantID = 0;
        Config = ConfigManager.GetBattleMomentConditionConfig(conditionID);
        ParamModel = paramModel;
        BuffLayerCount = layerCount;
        return OnCondition();
    }
    
    public bool Condition(int conditionID, BattleUnit subject, BattleUnit target, int skillGuid, MomentParamModel paramModel)
    {
        Subject = subject;
        Target = target;
        SpellCaster = null;
        InitActionTarget();
        SkillID = skillGuid / 10000;
        VariantID = skillGuid % 10000;
        Config = ConfigManager.GetBattleMomentConditionConfig(conditionID);
        ParamModel = paramModel;
        BuffLayerCount = 0;
        return OnCondition();
    }
    
    private void InitActionTarget()
    {
        if (ParamModel is DamageParamModel model)
        {
            if (model.AttackID == Subject.EntityID)
            {
                ActionTarget = BattleManager.GetUnit(model.HitID);
            }
            else if (model.HitID == Subject.EntityID)
            {
                ActionTarget = BattleManager.GetUnit(model.AttackID);
            }
            else
            {
                ActionTarget = null;
            }
        }
        else
        {
            ActionTarget = null;
        }
    }

    protected abstract bool OnCondition();
    
    protected BattleUnit GetUnitByParamID(float paramID)
    {
        return paramID.ToInt() switch
        {
            1 => Subject,
            2 => Target,
            3 => SpellCaster,
            _ => null
        };
    }
}