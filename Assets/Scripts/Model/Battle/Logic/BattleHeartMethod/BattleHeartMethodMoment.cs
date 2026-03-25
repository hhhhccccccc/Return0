using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethodMoment : IBattleMoment
{
    
    [Inject] private IPoolManager PM { get; set; }
    [Inject] private BattleMomentManager BattleMomentManager { get; set; }
    [Inject] private BattleRecordManager BattleRecordManager { get; set; }

    private BattleHeartMethodBase Model { get; set; }

    protected void InitMoment(BattleHeartMethodBase model)
    {
        Model = model;
    }

    public virtual void BattleStart()
    {
        
    }

    public virtual void RoundStart()
    { 
       
    }

    public void CalculateActionWheel()
    {
        
    }

    public void BeforeDoDesitionAction()
    {
        
    }

    public virtual void DoDesitionAction(bool isPreDesition)
    { 
        
    }

    public virtual void EveryActionWheelStart()
    {
        
    }

    public void SelfActionWheelStart()
    {
        
    }

    public void BeforeAction()
    { 
        
    }
    
    public void BeforeUnderAction()
    { 
       
    }
    
    public virtual void BeforeClash(MomentParamModel paramModel)
    { 
        
    }
    
    public virtual void AfterClash(MomentParamModel paramModel)
    { 
        
    }

    public virtual void ReleaseSkillAction(MomentParamModel paramModel)
    { 
        
    }
    
    public virtual void AfterUnderAction(MomentParamModel paramModel)
    {
        
    }
    
    public virtual void AfterAction(MomentParamModel paramModel)
    { 
        
    }

    public void ActionWheelEnd()
    {
        
    }

    public virtual void RoundEnd()
    { 
        
    }

    public void BattleEnd()
    {
        
    }

    public void EnqueueViewModel(BattleMomentViewModel viewModel)
    {
        BattleRecordManager.AddBattleMomentViewModel(viewModel);
    }

    public BattleMomentViewModel AllocViewModel(int entityID, MomentViewType viewType)
    {
        var viewModel = PM.GetClass<BattleMomentViewModel>();
        viewModel.BattleSource = BattleSource.HeartMethod;
        viewModel.EntityID = entityID;
        viewModel.ConfigID = Model.HeartMethodID;
        return viewModel;
    }
}
