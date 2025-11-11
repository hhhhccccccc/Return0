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
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.BattleStartMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.BattleStart));
        }
    }

    public void RoundStart()
    {
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.RoundStartMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.RoundStart));
        }
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

    public void DoDesitionAction()
    {
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.DoDesitionMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.DoDesitionAction));
        }
    }

    public void AfterEveryActionWheelStart()
    {
        
    }

    public void AfterSelfActionWheelStart()
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

    public void BeforeClash(MomentParamModel paramModel)
    {
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.BeforeClashMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel, BattleMomentType.BeforeClash));
        }
    }
    
    public void AfterClash(MomentParamModel paramModel)
    {
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.AfterClashMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel, BattleMomentType.AfterClash));
        }
    }
    
    public void ReleaseSkillAction(MomentParamModel paramModel)
    {
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.ReleaseSkillActionMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel, BattleMomentType.ReleaseSkillAction));
        }
    }

    public void AfterUnderAction(MomentParamModel paramModel)
    {
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.AfterUnderActionMoment)
        {
            EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel, BattleMomentType.AfterUnderAction));
        }
    }
    
    public void AfterAction(MomentParamModel paramModel)
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
            viewModel.BattleSource = BattleSource.Treasure;
            viewModel.ConfigID = Model.Config.Id;
            BattleRecordManager.AddBattleMomentViewModel(viewModel);
        }
    }
}