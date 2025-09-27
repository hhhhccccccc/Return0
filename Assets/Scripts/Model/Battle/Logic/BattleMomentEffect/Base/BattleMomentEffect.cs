using cfg;
using Codice.LogWrapper;
using Zenject;

public abstract class BattleMomentEffect : IModel
{
    [Inject] protected ConfigManager ConfigManager;
    [Inject] protected BattleLogicBehaviourManager BattleLogicBehaviourManager;
    [Inject] protected BattleManager BattleManager;
    [Inject] protected IPoolManager PoolManager;
    [Inject] protected ILogManager LogManager;
    protected void Debug(string msg) => LogManager.Debug(msg);
    protected BattleUnit Subject;
    protected BattleUnit Target;
    protected BattleUnit SpellCaster;
    protected BattleUnit ClashTarget;
    protected MomentParamModel ParamModel;
    protected BattleMomentEffectConfig Config;
    
    protected BattleMomentViewModel BattleMomentViewModel;
    protected int BuffLayerCount;
    public BattleMomentViewModel Effect(int momentEffectID, BattleUnit subject, BattleUnit target, MomentParamModel paramModel)
    {
        BattleMomentViewModel = PoolManager.GetClass<BattleMomentViewModel>();
        Subject = subject;
        Target = target;
        ParamModel = paramModel;
        InitClashTarget();
        Config = ConfigManager.GetBattleMomentEffectConfig(momentEffectID);
        BuffLayerCount = 0;
        OnEffect();
        ProcessViewModel();
        return BattleMomentViewModel;
    }
    
    public BattleMomentViewModel Effect(int momentEffectID, BattleUnit subject, BattleUnit target, BattleUnit spellCaster, MomentParamModel paramModel, int layerCount)
    {
        BattleMomentViewModel = PoolManager.GetClass<BattleMomentViewModel>();
        Subject = subject;
        Target = target;
        SpellCaster = spellCaster;
        ParamModel = paramModel;
        InitClashTarget();
        Config = ConfigManager.GetBattleMomentEffectConfig(momentEffectID);
        BuffLayerCount = layerCount;
        OnEffect();
        ProcessViewModel();
        return BattleMomentViewModel;
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

    protected abstract void OnEffect();

    protected virtual void ProcessViewModel()
    {
        
    }

    protected BattleUnit GetUnitByParamID(float paramID)
    {
        return paramID.ToInt() switch
        {
            1 => Subject,
            2 => Target,
            3 => SpellCaster,
            4 => ClashTarget,
            _ => null
        };
    }
}