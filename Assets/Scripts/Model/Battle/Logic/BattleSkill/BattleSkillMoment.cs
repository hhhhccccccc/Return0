using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleSkillMoment : IBattleMoment
{
    [Inject] protected IPoolManager PM { get; set; }
    [Inject] protected BattleMomentManager BattleMomentManager { get; set; }
    [Inject] protected BattleRecordManager BattleRecordManager { get; set; }
    
    private BattleSkillBase Model { get; set; }

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
            BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.CalculateActionWheel);
        }
    }

    public void BeforeDoDesitionAction()
    {
        
    }

    public virtual void DoDesitionAction(bool isPreDesition)
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.DoDesitionMoment)
        {
            BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.DoDesitionAction);
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
            BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.ActionWheelStart);
        }
    }

    public void BeforeAction()
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.BeforeActionMoment)
        {
            BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.BeforeAction);
        }
    }
    
    public void BeforeUnderAction()
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.BeforeUnderActionMoment)
        {
            BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.BeforeUnderAction);
        }
    }

    public virtual void BeforeClash(MomentParamModel paramModel)
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.BeforeClashMoment)
        {
            BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel, BattleMomentType.BeforeClash);
        }
    }
    
    public virtual void AfterClash(MomentParamModel paramModel)
    {  
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.AfterClashMoment)
        {
            BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel, BattleMomentType.AfterClash);
        }
    }
    
    public virtual void ReleaseSkillAction(MomentParamModel paramModel)
    {   
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.ReleaseSkillActionMoment)
        {
            BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel, BattleMomentType.ReleaseSkillAction);
        }
    }
    public virtual void AfterUnderAction(MomentParamModel paramModel)
    {
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.AfterUnderActionMoment)
        {
            BattleMomentManager.TriggerMoment(momentID, subjectID, paramModel, BattleMomentType.AfterUnderAction);
        }
    }
    
    public virtual void AfterAction(MomentParamModel paramModel)
    {   
        var subjectID = Model.Subject.EntityID;
        foreach (var momentID in Model.Config.AfterActionMoment)
        {
            BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.AfterAction);
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
            BattleMomentManager.TriggerMoment(momentID, subjectID, null, BattleMomentType.RoundEnd);
        }
    }

    public void BattleEnd()
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