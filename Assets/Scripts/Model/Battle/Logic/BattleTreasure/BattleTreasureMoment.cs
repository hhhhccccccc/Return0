using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public abstract class BattleTreasureMoment : IBattleMoment
{
    [Inject] private BattleMomentManager BattleMomentManager;
    [Inject] protected BattleRecordManager BattleRecordManager;

    private BattleTreasureBase Model;

    protected void InitMoment(BattleTreasureBase model)
    {
        Model = model;
    }

    public void BattleStart()
    {
        
    }

    public void RoundStart()
    {
        
    }

    public void CalculateActionWheel()
    {
       
    }

    public void BeforeDoDesitionAction()
    {
        
    }

    public void DoDesitionAction()
    {
      
    }

    public void EveryActionWheelStart()
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

    public void BeforeClash(MomentParamModel paramModel)
    {
    
    }
    
    public void AfterClash(MomentParamModel paramModel)
    {
       
    }
    
    public void ReleaseSkillAction(MomentParamModel paramModel)
    {
       
    }

    public void AfterUnderAction(MomentParamModel paramModel)
    {
        
    }
    
    public void AfterAction(MomentParamModel paramModel)
    {
       
    }

    public void ActionWheelEnd()
    {
        
    }

    public void RoundEnd()
    {
        
    }

    public void EnqueueViewModel(Queue<BattleMomentViewModel> viewModelQueue)
    {
        while (viewModelQueue.Any())
        {
            var viewModel = viewModelQueue.Dequeue();
            viewModel.BattleSource = BattleSource.Treasure;
            viewModel.ConfigID = Model.Config.Id;
            BattleRecordManager.AddBattleMomentViewModel(viewModel);
        }
    }
}