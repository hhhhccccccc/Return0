using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleBuffMoment : IBattleMoment
{
    [Inject] private BattleMomentManager BattleMomentManager;
    [Inject] protected BattleRecordManager BattleRecordManager;
    [Inject] private IPoolManager Poolmanager;

    protected BattleBuffBase Model;

    private Action<BuffReduceType, MomentParamModel> ReduceLayerImpl;
    protected void InitMoment(BattleBuffBase model, Action<BuffReduceType, MomentParamModel> reduceLayerImpl)
    {
        Model = model;
        ReduceLayerImpl = reduceLayerImpl;
    }

    public virtual void BattleStart()
    {
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.BattleStartMoment)
        {
            EnqueueViewModel(BattleMomentType.BattleStart, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }

        ReduceLayerCountByMoment(BattleMomentType.BattleStart);
    }

    public virtual void RoundStart()
    {
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.RoundStartMoment)
        {
            EnqueueViewModel(BattleMomentType.RoundStart, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        ReduceLayerCountByMoment(BattleMomentType.RoundStart);
    }

    public void CalculateActionWheel()
    {
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.CalculateActionWheelMoment)
        {
            EnqueueViewModel(BattleMomentType.CalculateActionWheel, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        ReduceLayerCountByMoment(BattleMomentType.CalculateActionWheel);
    }

    public virtual void DoDesitionAction()
    {  
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.DoDesitionMoment)
        {
            EnqueueViewModel(BattleMomentType.DoDesitionAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        ReduceLayerCountByMoment(BattleMomentType.DoDesitionAction);
    }

    public void EveryActionWheelStart()
    {
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.EveryActionWheelStartMoment)
        {
            EnqueueViewModel(BattleMomentType.EveryActionWheelStart, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        ReduceLayerCountByMoment(BattleMomentType.EveryActionWheelStart);
    }

    public virtual void ActionWheelStart()
    {
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.ActionWheelStartMoment)
        {
            EnqueueViewModel(BattleMomentType.ActionWheelStart, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        ReduceLayerCountByMoment(BattleMomentType.ActionWheelStart);
    }

    public virtual void BeforeAction()
    {
        if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
        {
            return;
        }
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.BeforeActionMoment)
        {
            EnqueueViewModel(BattleMomentType.BeforeAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        ReduceLayerCountByMoment(BattleMomentType.BeforeAction);
    }

    public virtual void BeforeUnderAction()
    {  
        if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
        {
            return;
        }
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.BeforeUnderActionMoment)
        {
            EnqueueViewModel(BattleMomentType.BeforeUnderAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        ReduceLayerCountByMoment(BattleMomentType.BeforeUnderAction);
    }

    public virtual void BeforeClash(MomentParamModel paramModel)
    {  
        if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
        {
            return;
        }
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.BeforeClashMoment)
        {
            EnqueueViewModel(BattleMomentType.BeforeClash, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount));
        }
        
        ReduceLayerCountByMoment(BattleMomentType.BeforeClash);
    }
    
    public virtual void AfterClash(MomentParamModel paramModel)
    {  
        if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
        {
            return;
        }
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in  Model.Config.AfterClashMoment)
        {
            EnqueueViewModel(BattleMomentType.AfterClash, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount));
        }
        
        ReduceLayerCountByMoment(BattleMomentType.AfterClash);
    }
    
    public virtual void ReleaseSkillAction(MomentParamModel paramModel)
    {  
        if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
        {
            return;
        }
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.ReleaseSkillActionMoment)
        {
            EnqueueViewModel(BattleMomentType.ReleaseSkillAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount));
        }
        
        ReduceLayerCountByMoment(BattleMomentType.ReleaseSkillAction, paramModel);
    }
    
    public virtual void AfterUnderAction(MomentParamModel paramModel)
    {
        if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
        {
            return;
        }
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.AfterUnderActionMoment)
        {
            EnqueueViewModel(BattleMomentType.AfterUnderAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount));
        }
        
        ReduceLayerCountByMoment(BattleMomentType.AfterUnderAction);
    }

    public virtual void AfterAction(MomentParamModel paramModel)
    {  
        if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
        {
            return;
        }
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.AfterActionMoment)
        {
            EnqueueViewModel(BattleMomentType.AfterAction, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount));
        }

        if (Model.Config.BeStatusPersists == 1)
        {
            if (Model.Subject.StatusPersists > 0)
            {
                return;
            }

            if (Model.Subject.GainStatusPersists > 0 && Model.BuffType == BuffType.Gain)
            {
                return;
            }
        }
        
        ReduceLayerCountByMoment(BattleMomentType.AfterAction);
    }

    public void ActionWheelEnd()
    {
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.ActionWheelEndMoment)
        {
            EnqueueViewModel(BattleMomentType.ActionWheelEnd, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        ReduceLayerCountByMoment(BattleMomentType.ActionWheelEnd);
    }

    public virtual void RoundEnd()
    {  
        var subjectID = Model.Subject.EntityID;
        var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
        foreach (var momentID in Model.Config.RoundEndMoment)
        {
            EnqueueViewModel(BattleMomentType.RoundEnd, BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount));
        }
        
        ReduceLayerCountByMoment(BattleMomentType.RoundEnd);
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
    private void ReduceLayerCountByMoment(BattleMomentType momentType, MomentParamModel paramModel = null)
    {
        var reduceMoment =  Model.Config.BuffLevelReduceMoment;
        for (int i = 0; i < reduceMoment.Count; i += 2)
        {
            var reduceMomentType = reduceMoment[i];
            if (reduceMomentType == (int)momentType)
            {
                ReduceLayerImpl?.Invoke((BuffReduceType)reduceMoment[i + 1], paramModel);
            }
        }
    }
}
