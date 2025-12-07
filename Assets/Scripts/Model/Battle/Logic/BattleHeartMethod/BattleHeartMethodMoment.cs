using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethodMoment : IBattleMoment
{
    [Inject] private BattleMomentManager BattleMomentManager;
    [Inject] private BattleRecordManager BattleRecordManager;

    private BattleHeartMethodBase Model;

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

    public virtual void DoDesitionAction()
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

    public void EnqueueViewModel(Queue<BattleMomentViewModel> viewModelQueue)
    {
        while (viewModelQueue.Any())
        {
            var viewModel = viewModelQueue.Dequeue();
            viewModel.BattleSource = BattleSource.HeartMethod;
            viewModel.ConfigID = Model.Config.Id;
            BattleRecordManager.AddBattleMomentViewModel(viewModel);
        }
    }
}
