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

    public void BattleStart()
    {
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.BattleStartMoment)
        {
            EnqueueViewModel(BattleMomentType.BattleStart, BattleMomentManager.TriggerMoment(momentID, subjectID, null));
        }
    }

    public void RoundStart()
    { 
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.RoundStartMoment)
        {
            EnqueueViewModel(BattleMomentType.RoundStart, BattleMomentManager.TriggerMoment(momentID, subjectID, null));
        }
    }

    public void CalculateActionWheel()
    {
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.CalculateActionWheelMoment)
        {
            EnqueueViewModel(BattleMomentType.CalculateActionWheel, BattleMomentManager.TriggerMoment(momentID, subjectID, null));
        }
    }

    public void DoDesitionAction()
    { 
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.DoDesitionMoment)
        {
            EnqueueViewModel(BattleMomentType.DoDesitionAction, BattleMomentManager.TriggerMoment(momentID, subjectID, null));
        }
    }

    public void ActionWheelStart()
    {
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.ActionWheelStartMoment)
        {
            EnqueueViewModel(BattleMomentType.ActionWheelStart, BattleMomentManager.TriggerMoment(momentID, subjectID, null));
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
            EnqueueViewModel(BattleMomentType.BeforeClash, BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel));
        }
    }
    
    public void AfterClash(MomentParamModel paramModel)
    { 
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.AfterClashMoment)
        {
            EnqueueViewModel(BattleMomentType.AfterClash, BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel));
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

    public void ActionWheelEnd()
    {
        
    }

    public void RoundEnd()
    { 
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.RoundEndMoment)
        {
            EnqueueViewModel(BattleMomentType.RoundEnd, BattleMomentManager.TriggerMoment(momentID, subjectID, null));
        }
    }

    public void EnqueueViewModel(BattleMomentType momentType, Queue<BattleMomentViewModel> viewModelQueue)
    {
        while (viewModelQueue.Any())
        {
            var viewModel = viewModelQueue.Dequeue();
            viewModel.BattleMomentType = momentType;
            viewModel.BattleSource = BattleSource.HeartMethod;
            viewModel.ConfigID = Model.Config.Id;
            BattleRecordManager.AddBattleMomentViewModel(viewModel);
        }
    }
}
