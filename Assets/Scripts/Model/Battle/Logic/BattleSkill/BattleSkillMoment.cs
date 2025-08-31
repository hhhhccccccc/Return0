using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleSkillMoment : IBattleMoment
{
    [Inject] protected BattleMomentManager BattleMomentManager;
    [Inject] protected BattleRecordManager BattleRecordManager;
    
    private BattleSkillBase Model;

    protected void InitMoment(BattleSkillBase model)
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

    public void DoDesitionAction()
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.DoDesitionMoment)
        {
            EnqueueViewModel(BattleMomentType.DoDesitionAction, BattleMomentManager.TriggerMoment(momentID, subjectID, null));
        }
    }

    public void BeforeAction()
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.BeforeActionMoment)
        {
            EnqueueViewModel(BattleMomentType.BeforeAction, BattleMomentManager.TriggerMoment(momentID, subjectID, null));
        }
    }
    
    public void BeforeUnderAction()
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.BeforeUnderActionMoment)
        {
            EnqueueViewModel(BattleMomentType.BeforeUnderAction, BattleMomentManager.TriggerMoment(momentID, subjectID, null));
        }
    }

    public void BeforeClash(MomentParamModel paramModel)
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.BeforeClashMoment)
        {
            EnqueueViewModel(BattleMomentType.BeforeClash, BattleMomentManager.TriggerMoment(momentID, subjectID, null));
        }
    }
    
    public void AfterClash(MomentParamModel paramModel)
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.AfterClashMoment)
        {
            EnqueueViewModel(BattleMomentType.AfterClash, BattleMomentManager.TriggerMoment(momentID, subjectID, null));
        }
    }
    
    public void ReleaseSkillAction(MomentParamModel paramModel)
    {   
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.ReleaseSkillActionMoment)
        {
            EnqueueViewModel(BattleMomentType.ReleaseSkillAction, BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel));
        }
    }
    public void AfterUnderAction(MomentParamModel paramModel)
    {
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.AfterUnderActionMoment)
        {
            EnqueueViewModel(BattleMomentType.AfterUnderAction, BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel));
        }
    }
    
    public void AfterAction(MomentParamModel paramModel)
    {   
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.AfterActionMoment)
        {
            EnqueueViewModel(BattleMomentType.AfterAction, BattleMomentManager.TriggerMoment(momentID, subjectID, null));
        }
    }

    public void RoundEnd()
    {
       
    }
    
    public void EnqueueViewModel(BattleMomentType momentType, Queue<BattleMomentViewModel> viewModelQueue)
    {
        while (viewModelQueue.Any())
        {
            var viewModel = viewModelQueue.Dequeue();
            viewModel.BattleMomentType = momentType;
            viewModel.BattleSource = BattleSource.Skill;
            viewModel.ConfigID = Model.Config.Id;
            BattleRecordManager.AddBattleMomentViewModel(viewModel);
        }
    }
}