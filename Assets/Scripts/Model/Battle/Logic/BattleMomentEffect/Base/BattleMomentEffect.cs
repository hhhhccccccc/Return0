using System.Collections.Generic;
using cfg;
using Codice.LogWrapper;
using Sirenix.Utilities;
using Zenject;

public abstract class BattleMomentEffect : IModel
{
    [Inject] protected ConfigManager ConfigManager { get; set; }
    [Inject] protected BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    [Inject] protected BattleManager BattleManager { get; set; }
    [Inject] protected IPoolManager PoolManager { get; set; }
    [Inject] protected ILogManager LogManager { get; set; }
    protected void Debug(string msg) => LogManager.Debug(msg);
    protected BattleUnit Subject { get; set; }
    protected BattleUnit Target { get; set; }
    protected BattleUnit SpellCaster { get; set; }
    protected BattleUnit ClashTarget { get; set; }
    protected MomentParamModel ParamModel { get; set; }
    protected BattleMomentEffectConfig Config { get; set; }
    
    protected BattleMomentViewModel BattleMomentViewModel { get; set; }
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

    private List<BattleUnit> TempUnitList = new();

    protected List<BattleUnit> GetUnitByParamID(float paramID)
    {
        TempUnitList.Clear();
        switch (paramID.ToInt())
        {
            case 1:
                if (Subject != null)
                {
                    TempUnitList.Add(Subject);
                }
                break;
            case 2:
                if (Target != null)
                {
                    TempUnitList.Add(Target);
                }
                break;
            case 3:
                if (SpellCaster != null)
                {
                    TempUnitList.Add(SpellCaster);
                }
                break;
            case 4:
                if (ClashTarget != null)
                {
                    TempUnitList.Add(ClashTarget);
                }
                break;
            case 5:
                TempUnitList.AddRange(BattleManager.GetAllTeamUnit(Subject.EntityID, true));
                break;
            case 6:
                TempUnitList.AddRange(BattleManager.GetAllOpponentUnit(Subject.EntityID, true));
                break;
            case 7:
                TempUnitList.AddRange(BattleManager.GetAllAliveUnit());
                break;
        }

        return TempUnitList;
    }
}