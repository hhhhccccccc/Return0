using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuffMoment : IBattleMoment
{
    [Inject] private BattleMomentManager BattleMomentManager;
    [Inject] protected BattleRecordManager BattleRecordManager;
    [Inject] private IPoolManager Poolmanager;

    private BattleBuffBase Model;
    public void InitMoment(BattleBuffBase model)
    {
        Model = model;
    }

    public virtual void BattleStart()
    {
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.BattleStartMoment)
        {
            EnqueueViewModel(BattleMomentType.BattleStart, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }

        TryReduceLayerCount(BattleMomentType.BattleStart);
    }

    public virtual void RoundStart()
    {
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.RoundStartMoment)
        {
            EnqueueViewModel(BattleMomentType.RoundStart, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        TryReduceLayerCount(BattleMomentType.RoundStart);
    }

    public void CalculateActionWheel()
    {
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.CalculateActionWheelMoment)
        {
            EnqueueViewModel(BattleMomentType.CalculateActionWheel, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        TryReduceLayerCount(BattleMomentType.CalculateActionWheel);
    }

    public virtual void DoDesitionAction()
    {  
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.DoDesitionMoment)
        {
            EnqueueViewModel(BattleMomentType.DoDesitionAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        TryReduceLayerCount(BattleMomentType.DoDesitionAction);
    }

    public virtual void ActionWheelStart()
    {
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.ActionWheelStartMoment)
        {
            EnqueueViewModel(BattleMomentType.ActionWheelStart, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        TryReduceLayerCount(BattleMomentType.ActionWheelStart);
    }

    public virtual void BeforeAction()
    { 
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.BeforeActionMoment)
        {
            EnqueueViewModel(BattleMomentType.BeforeAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        TryReduceLayerCount(BattleMomentType.BeforeAction);
    }

    public virtual void BeforeUnderAction()
    {  
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.BeforeUnderActionMoment)
        {
            EnqueueViewModel(BattleMomentType.BeforeUnderAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        TryReduceLayerCount(BattleMomentType.BeforeUnderAction);
    }

    public virtual void BeforeClash(MomentParamModel paramModel)
    {  
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.BeforeClashMoment)
        {
            EnqueueViewModel(BattleMomentType.BeforeClash, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount));
        }
        
        TryReduceLayerCount(BattleMomentType.BeforeClash);
    }
    
    public virtual void AfterClash(MomentParamModel paramModel)
    {  
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in  Model.Config.AfterClashMoment)
        {
            EnqueueViewModel(BattleMomentType.AfterClash, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount));
        }
        
        TryReduceLayerCount(BattleMomentType.AfterClash);
    }
    
    public virtual void ReleaseSkillAction(MomentParamModel paramModel)
    {  
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.ReleaseSkillActionMoment)
        {
            EnqueueViewModel(BattleMomentType.ReleaseSkillAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount));
        }
        
        TryReduceLayerCount(BattleMomentType.ReleaseSkillAction, paramModel);
    }
    
    public virtual void AfterUnderAction(MomentParamModel paramModel)
    {
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.AfterUnderActionMoment)
        {
            EnqueueViewModel(BattleMomentType.AfterUnderAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount));
        }
        
        TryReduceLayerCount(BattleMomentType.AfterUnderAction);
    }

    public virtual void AfterAction(MomentParamModel paramModel)
    {  
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.AfterActionMoment)
        {
            EnqueueViewModel(BattleMomentType.AfterAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID, paramModel, Model.LayerCount));
        }
        
        TryReduceLayerCount(BattleMomentType.AfterAction);
    }

    public virtual void RoundEnd()
    {  
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.RoundEndMoment)
        {
            EnqueueViewModel(BattleMomentType.RoundEnd, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        TryReduceLayerCount(BattleMomentType.RoundEnd);
    }

    public void EnqueueViewModel(BattleMomentType momentType, Queue<BattleMomentViewModel> viewModelQueue)
    {
        while (viewModelQueue.Any())
        {
            var viewModel = viewModelQueue.Dequeue();
            viewModel.BattleMomentType = momentType;
            viewModel.BattleSource = BattleSource.Buff;
            viewModel.ConfigID = Model.Config.ID;
            BattleRecordManager.AddBattleMomentViewModel(viewModel);
        }
    }

    /// <summary>
    /// 减少buff持续时间
    /// </summary>
    public void TryReduceLayerCount(BattleMomentType momentType, MomentParamModel paramModel = null)
    {
        var reduceMoment =  Model.Config.BuffLevelReduceMoment;
        for (int i = 0; i < reduceMoment.Count; i += 2)
        {
            var reduceMomentType = reduceMoment[i];
            if (reduceMomentType == (int)momentType)
            {
                Model.ReduceLayer((BuffReduceType)reduceMoment[i + 1], paramModel);
            }
        }
    }
}
