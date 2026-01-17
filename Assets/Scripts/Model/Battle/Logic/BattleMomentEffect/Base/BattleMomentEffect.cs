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
    protected void Debug(string msg) => LogManager.D(msg);
    protected BattleUnit Subject { get; set; }
    protected BattleUnit Target { get; set; }
    protected BattleUnit SpellCaster { get; set; }
    protected BattleUnit ActionTarget { get; set; }
    protected MomentParamModel ParamModel { get; set; }
    protected BattleMomentEffectConfig Config { get; set; }
    protected int BuffLayerCount { get; set; }
    protected BattleMomentType MomentType { get; set; }
    public void Effect(int momentEffectID, BattleUnit subject, BattleUnit target, MomentParamModel paramModel, BattleMomentType momentType)
    {
        Subject = subject;
        Target = target;
        ParamModel = paramModel;
        MomentType = momentType;
        InitActionTarget();
        Config = ConfigManager.GetBattleMomentEffectConfig(momentEffectID);
        BuffLayerCount = 0;
        OnEffect();
        ProcessViewModel();
    }
    
    public void Effect(int momentEffectID, BattleUnit subject, BattleUnit target, BattleUnit spellCaster, MomentParamModel paramModel, int layerCount, BattleMomentType momentType)
    {
        Subject = subject;
        Target = target;
        SpellCaster = spellCaster;
        ParamModel = paramModel;
        MomentType = momentType;
        InitActionTarget();
        Config = ConfigManager.GetBattleMomentEffectConfig(momentEffectID);
        BuffLayerCount = layerCount;
        OnEffect();
        ProcessViewModel();
    }

    private void InitActionTarget()
    {
        if (ParamModel is DamageParamModel model)
        {
            ActionTarget = BattleManager.GetUnit(model.GetOtherID(Subject.EntityID));
        }
        else
        {
            ActionTarget = null;
        }
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
                if (ActionTarget != null)
                {
                    TempUnitList.Add(ActionTarget);
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
            case 8:
                TempUnitList.AddRange(BattleManager.GetAllAliveUnit());
                if (TempUnitList.Contains(Subject))
                {
                    TempUnitList.Remove(Subject);
                }
                break;
        }

        return TempUnitList;
    }
}