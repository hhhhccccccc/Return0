using cfg;
using Zenject;

public abstract class BattleMomentEffect : IModel
{
    [Inject] protected ConfigManager ConfigManager;
    [Inject] protected BattleLogicBehaviourManager BattleLogicBehaviourManager;
    [Inject] protected IPoolManager PoolManager;
    protected BattleUnit Subject;
    protected BattleUnit Target;
    protected BattleUnit Spellcaster;
    protected MomentParamModel ParamModel;
    protected BattleMomentEffectConfig Config;
    
    protected BattleMomentViewModel BattleMomentViewModel;
    public BattleMomentViewModel Effect(int momentEffectID, BattleUnit subject, BattleUnit target, MomentParamModel paramModel)
    {
        BattleMomentViewModel = PoolManager.GetClass<BattleMomentViewModel>();
        Subject = subject;
        Target = target;
        ParamModel = paramModel;
        Config = ConfigManager.GetBattleMomentEffectConfig(momentEffectID);
        OnEffect();
        ProcessViewModel();
        return BattleMomentViewModel;
    }
    
    public BattleMomentViewModel Effect(int momentEffectID, BattleUnit subject, BattleUnit target, BattleUnit spellcaster, MomentParamModel paramModel)
    {
        BattleMomentViewModel = PoolManager.GetClass<BattleMomentViewModel>();
        Subject = subject;
        Target = target;
        Spellcaster = spellcaster;
        ParamModel = paramModel;
        Config = ConfigManager.GetBattleMomentEffectConfig(momentEffectID);
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
            3 => Spellcaster,
            _ => null
        };
    }
}