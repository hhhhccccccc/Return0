using cfg;
using Zenject;

public abstract class BattleMomentCondition : IModel
{
    [Inject] protected ConfigManager ConfigManager  { get; set; }
    [Inject] protected BattleManager BattleManager  { get; set; }
    protected BattleUnit Subject  { get; set; }
    protected BattleUnit Target  { get; set; }
    protected BattleUnit SpellCaster  { get; set; }
    protected BattleUnit ClashTarget  { get; set; }
    protected BattleMomentConditionConfig Config  { get; set; }
    protected MomentParamModel ParamModel  { get; set; }
    protected int SkillID  { get; set; }
    protected int BuffLayerCount  { get; set; }
    public bool Condition(int conditionID, BattleUnit subject, BattleUnit target, MomentParamModel paramModel)
    {
        Subject = subject;
        Target = target;
        SpellCaster = null;
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
        SpellCaster = spellcaster;
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
        SpellCaster = null;
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
            3 => SpellCaster,
            _ => null
        };
    }
}