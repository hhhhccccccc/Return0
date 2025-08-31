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

    public void BattleStart()
    {
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.BattleStartMoment)
        {
            EnqueueViewModel(BattleMomentType.BattleStart, BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID, null));
        }

        TryReduceLevel(BattleMomentType.BattleStart);
    }

    public void RoundStart()
    {
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.RoundStartMoment)
        {
            EnqueueViewModel(BattleMomentType.RoundStart, BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID, null));
        }
        
        TryReduceLevel(BattleMomentType.RoundStart);
    }

    public void DoDesitionAction()
    {  
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.DoDesitionMoment)
        {
            EnqueueViewModel(BattleMomentType.DoDesitionAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID, null));
        }
        
        TryReduceLevel(BattleMomentType.DoDesitionAction);
    }

    public void BeforeAction()
    { 
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.BeforeActionMoment)
        {
            EnqueueViewModel(BattleMomentType.BeforeAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID, null));
        }
        
        TryReduceLevel(BattleMomentType.BeforeAction);
    }

    public void BeforeUnderAction()
    {  
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.BeforeUnderActionMoment)
        {
            EnqueueViewModel(BattleMomentType.BeforeUnderAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID, null));
        }
        
        TryReduceLevel(BattleMomentType.BeforeUnderAction);
    }

    public void BeforeClash(MomentParamModel paramModel)
    {  
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.BeforeClashMoment)
        {
            EnqueueViewModel(BattleMomentType.BeforeClash, BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID, paramModel));
        }
        
        TryReduceLevel(BattleMomentType.BeforeClash);
    }
    
    public void AfterClash(MomentParamModel paramModel)
    {  
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in  Model.Config.AfterClashMoment)
        {
            EnqueueViewModel(BattleMomentType.AfterClash, BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID, paramModel));
        }
        
        TryReduceLevel(BattleMomentType.AfterClash);
    }
    
    public void ReleaseSkillAction(MomentParamModel paramModel)
    {  
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.ReleaseSkillActionMoment)
        {
            EnqueueViewModel(BattleMomentType.ReleaseSkillAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID, paramModel));
        }
        
        TryReduceLevel(BattleMomentType.ReleaseSkillAction, paramModel);
    }
    
    public void AfterUnderAction(MomentParamModel paramModel)
    {
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.AfterUnderActionMoment)
        {
            EnqueueViewModel(BattleMomentType.AfterUnderAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID, paramModel));
        }
        
        TryReduceLevel(BattleMomentType.AfterUnderAction);
    }

    public void AfterAction(MomentParamModel paramModel)
    {  
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.AfterActionMoment)
        {
            EnqueueViewModel(BattleMomentType.AfterAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID, paramModel));
        }
        
        TryReduceLevel(BattleMomentType.AfterAction);
    }

    public void RoundEnd()
    {  
        var subjectID = Model.Subject.EntityID;
        var spellcasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.RoundEndMoment)
        {
            EnqueueViewModel(BattleMomentType.RoundEnd, BattleMomentManager.TriggerMoment(momentID, subjectID, spellcasterID, null));
        }
        
        TryReduceLevel(BattleMomentType.RoundEnd);
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
    protected void TryReduceLevel(BattleMomentType momentType, MomentParamModel paramModel = null)
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
