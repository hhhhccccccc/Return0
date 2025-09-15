using cfg;
using Codice.LogWrapper;
using Zenject;

public abstract class BattleMomentEffect : IModel
{
    [Inject] protected ConfigManager ConfigManager;
    [Inject] protected BattleLogicBehaviourManager BattleLogicBehaviourManager;
    [Inject] protected IPoolManager PoolManager;
    [Inject] protected ILogManager LogManager;
    protected void Debug(string msg) => LogManager.Debug(msg);
    protected BattleUnit Subject;
    protected BattleUnit Target;
    protected BattleUnit SpellCaster;
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
        Config = ConfigManager.GetBattleMomentEffectConfig(momentEffectID);
        BuffLayerCount = layerCount;
        OnEffect();
        ProcessViewModel();
        return BattleMomentViewModel;
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
            _ => null
        };
    }
}