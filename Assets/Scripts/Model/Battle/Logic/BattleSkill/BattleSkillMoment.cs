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
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.CalculateActionWheelMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.CalculateActionWheel));
        }
    }

    public void BeforeDoDesitionAction()
    {
        
    }

    public virtual void DoDesitionAction()
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.DoDesitionMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.DoDesitionAction));
        }
    }

    public void EveryActionWheelStart()
    {
        
    }

    public virtual void SelfActionWheelStart()
    {
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.ActionWheelStartMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.ActionWheelStart));
        }
    }

    public void BeforeAction()
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.BeforeActionMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.BeforeAction));
        }
    }
    
    public void BeforeUnderAction()
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.BeforeUnderActionMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.BeforeUnderAction));
        }
    }

    public virtual void BeforeClash(MomentParamModel paramModel)
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.BeforeClashMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel, BattleMomentType.BeforeClash));
        }
    }
    
    public virtual void AfterClash(MomentParamModel paramModel)
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.AfterClashMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel, BattleMomentType.AfterClash));
        }
    }
    
    public virtual void ReleaseSkillAction(MomentParamModel paramModel)
    {   
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.ReleaseSkillActionMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel, BattleMomentType.ReleaseSkillAction));
        }
    }
    public virtual void AfterUnderAction(MomentParamModel paramModel)
    {
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.AfterUnderActionMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel, BattleMomentType.AfterUnderAction));
        }
    }
    
    public virtual void AfterAction(MomentParamModel paramModel)
    {   
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.AfterActionMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.AfterAction));
        }
    }

    public void ActionWheelEnd()
    {
        
    }

    public void RoundEnd()
    {
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.RoundEndMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.RoundEnd));
        }
    }
    
    public void EnqueueViewModel(Queue<BattleMomentViewModel> viewModelQueue)
    {
        while (viewModelQueue.Any())
        {
            var viewModel = viewModelQueue.Dequeue();
            viewModel.BattleSource = BattleSource.Skill;
            viewModel.ConfigID = Model.Config.Id;
            BattleRecordManager.AddBattleMomentViewModel(viewModel);
        }
    }
}