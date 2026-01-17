using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public abstract class BattleTreasureMoment : IBattleMoment
{
    [Inject] protected BattleManager BattleManager { get; set; }
    [Inject] protected BattleMomentManager BattleMomentManager { get; set; }
    [Inject] protected BattleRecordManager BattleRecordManager { get; set; }
    [Inject] protected IPoolManager PM { get; set; }
    private BattleTreasureBase Model { get; set; }

    protected void InitMoment(BattleTreasureBase model)
    {
        Model = model;
    }
    
    protected bool CanTrigger()
    {
        var aliveUnitList = BattleManager.GetAllAliveUnit();
        if (aliveUnitList.Any(unit => unit.BattleChangeModelManager.CheckHasMethod(GameConst.Battle.HeartMethod10095)))
        {
            return false;
        }

        return true;
    }

    public void BattleStart()
    {
        if (!CanTrigger())
        {
            return;
        }

        OnBattleStart();
    }

    protected virtual void OnBattleStart()
    {
        
    }

    public void RoundStart()
    {
        if (!CanTrigger())
        {
            return;
        }

        OnRoundStart();
    }
    
    protected virtual void OnRoundStart()
    {
        
    }

    public void CalculateActionWheel()
    {
        if (!CanTrigger())
        {
            return;
        }

        OnCalculateActionWheel();
    }
    
    protected virtual void OnCalculateActionWheel()
    {
        
    }

    public void BeforeDoDesitionAction()
    {
        if (!CanTrigger())
        {
            return;
        }

        OnBeforeDoDesitionAction();
    }
    
    protected virtual void OnBeforeDoDesitionAction()
    {
        
    }
    
    public void DoDesitionAction(bool isPreDesition)
    {
        if (!CanTrigger())
        {
            return;
        }

        OnDoDesitionAction(isPreDesition);
    }
    
    protected virtual void OnDoDesitionAction(bool isPreDesition)
    {
        
    }

    public void EveryActionWheelStart()
    {
        if (!CanTrigger())
        {
            return;
        }

        OnEveryActionWheelStart();
    }
    
    protected virtual void OnEveryActionWheelStart()
    {
        
    }

    public void SelfActionWheelStart()
    {
        if (!CanTrigger())
        {
            return;
        }

        OnSelfActionWheelStart();
    }
    
    protected virtual void OnSelfActionWheelStart()
    {
        
    }

    public void BeforeAction()
    {
        if (!CanTrigger())
        {
            return;
        }

        OnBeforeAction();
    }
    
    protected virtual void OnBeforeAction()
    {
        
    }
    
    public void BeforeUnderAction()
    {
        if (!CanTrigger())
        {
            return;
        }

        OnBeforeUnderAction();
    }
    
    protected virtual void OnBeforeUnderAction()
    {
        
    }

    public void BeforeClash(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return;
        }

        OnBeforeClash(paramModel);
    }
    
    protected virtual void OnBeforeClash(MomentParamModel paramModel)
    {
        
    }
    
    public void AfterClash(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return;
        }

        OnAfterClash(paramModel);
    }
    
    protected virtual void OnAfterClash(MomentParamModel paramModel)
    {
        
    }
    
    public void ReleaseSkillAction(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return;
        }
        
        OnReleaseSkillAction(paramModel);
    }
    
    protected virtual void OnReleaseSkillAction(MomentParamModel paramModel)
    {
        
    }

    public void AfterUnderAction(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return;
        }

        OnAfterUnderAction(paramModel);
    }
    
    protected virtual void OnAfterUnderAction(MomentParamModel paramModel)
    {
        
    }
    
    public void AfterAction(MomentParamModel paramModel)
    {
        if (!CanTrigger())
        {
            return;
        }

        OnAfterAction(paramModel);
    }
    
    protected virtual void OnAfterAction(MomentParamModel paramModel)
    {
        
    }

    public void ActionWheelEnd()
    {
        if (!CanTrigger())
        {
            return;
        }

        OnActionWheelEnd();
    }
    
    protected virtual void OnActionWheelEnd()
    {
        
    }

    public void RoundEnd()
    {
        if (!CanTrigger())
        {
            return;
        }
        
        OnRoundEnd();
    }
    
    protected virtual void OnRoundEnd()
    {
        
    }

    public void BattleEnd()
    {
        if (!CanTrigger())
        {
            return;
        }

        OnBattleEnd();
    }
    
    protected virtual void OnBattleEnd()
    {
        
    }
    
    public void EnqueueViewModel(BattleMomentViewModel viewModel)
    {
        BattleRecordManager.AddBattleMomentViewModel(viewModel);
    }

    public BattleMomentViewModel AllocViewModel()
    {
        return PM.GetClass<BattleMomentViewModel>();
    }
}