using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;


/// <summary>
/// 如果扳机会被无法增益所影响 直接配表就行了 外层已经回绝掉所有受无法增益影响的扳机  没有On的直接走 有On的需要判断是否触发才能走
/// </summary>
public class BattleBuffMoment : IBattleMoment
{
    [Inject] private BattleMomentManager BattleMomentManager { get; set; }
    [Inject] protected BattleRecordManager BattleRecordManager { get; set; }
    [Inject] private IPoolManager Poolmanager { get; set; }

    protected BattleBuffBase Model { get; set; }
    
    protected void InitMoment(BattleBuffBase model)
    {
        Model = model;
    }

    public virtual void BattleStart()
    {
        if (CanTriggerBuffEffect())
        {
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.BattleStartMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount, BattleMomentType.BattleStart));
            }

            OnBattleStart();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.BattleStart);
    }
    protected virtual void OnBattleStart() {}

    public virtual void RoundStart()
    {
        if (CanTriggerBuffEffect())
        {
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.RoundStartMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount, BattleMomentType.RoundStart));
            }
            
            OnRoundStart();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.RoundStart);
    }
    protected virtual void OnRoundStart() {}
    
    public void CalculateActionWheel()
    {
        if (CanTriggerBuffEffect())
        {
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.CalculateActionWheelMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount, BattleMomentType.CalculateActionWheel));
            }

            OnCalculateActionWheel();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.CalculateActionWheel);
    }
    protected virtual void OnCalculateActionWheel() {}
    
    public void BeforeDoDesitionAction()
    {
        if (CanTriggerBuffEffect())
        {
            OnBeforeDoDesitionAction();
        }
    }
    protected virtual void OnBeforeDoDesitionAction() { }
    
    public virtual void DoDesitionAction()
    {  
        if (CanTriggerBuffEffect())
        {
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.DoDesitionMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount, BattleMomentType.DoDesitionAction));
            }
            
            OnDoDesitionAction();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.DoDesitionAction);
    }
    protected virtual void OnDoDesitionAction() {}
    
    public void AfterEveryActionWheelStart()
    {
        if (CanTriggerBuffEffect())
        {
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.EveryActionWheelStartMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount, BattleMomentType.EveryActionWheelStart));
            }

            OnAfterEveryActionWheelStart();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.EveryActionWheelStart);
    }
    protected virtual void OnAfterEveryActionWheelStart() {}

    public virtual void AfterSelfActionWheelStart()
    {
        if (CanTriggerBuffEffect())
        {
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.ActionWheelStartMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount, BattleMomentType.ActionWheelStart));
            }
            
            OnAfterSelfActionWheelStart();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.ActionWheelStart);
    }
    protected virtual void OnAfterSelfActionWheelStart() {}

    public virtual void BeforeAction()
    {
        if (CanTriggerBuffEffect())
        {
            if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
            {
                return;
            }
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.BeforeActionMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount, BattleMomentType.BeforeAction));
            }

            OnBeforeAction();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.BeforeAction);
    }
    protected virtual void OnBeforeAction() {}

    public virtual void BeforeUnderAction()
    {  
        if (CanTriggerBuffEffect())
        {
            if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
            {
                return;
            }
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.BeforeUnderActionMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount, BattleMomentType.BeforeUnderAction));
            }

            OnBeforeUnderAction();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.BeforeUnderAction);
    }
    protected virtual void OnBeforeUnderAction(){}
    
    public virtual void BeforeClash(MomentParamModel paramModel)
    {  
        if (CanTriggerBuffEffect())
        {
            if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
            {
                return;
            }
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.BeforeClashMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount, BattleMomentType.BeforeClash));
            }

            OnBeforeClash(paramModel);
        }
        
        ReduceLayerCountByMoment(BattleMomentType.BeforeClash);
    }
    protected virtual void OnBeforeClash(MomentParamModel paramModel) {}

    public virtual void AfterClash(MomentParamModel paramModel)
    {  
        if (CanTriggerBuffEffect())
        {
            if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
            {
                return;
            }
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in  Model.Config.AfterClashMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount, BattleMomentType.AfterClash));
            }

            OnAfterClash(paramModel);
        }
        
        ReduceLayerCountByMoment(BattleMomentType.AfterClash);
    }
    protected virtual void OnAfterClash(MomentParamModel paramModel) {}
    
    public virtual void ReleaseSkillAction(MomentParamModel paramModel)
    {  
        if (CanTriggerBuffEffect())
        {
            if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
            {
                return;
            }
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.ReleaseSkillActionMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount, BattleMomentType.ReleaseSkillAction));
            }

            OnReleaseSkillAction(paramModel);
        }
        
        
        ReduceLayerCountByMoment(BattleMomentType.ReleaseSkillAction, paramModel);
    }
    protected virtual void OnReleaseSkillAction(MomentParamModel paramModel) {}
    
    public virtual void AfterUnderAction(MomentParamModel paramModel)
    {
        if (CanTriggerBuffEffect())
        {
            if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
            {
                return;
            }
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.AfterUnderActionMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount, BattleMomentType.AfterUnderAction));
            }

            OnAfterUnderAction(paramModel);
        }
       
        
        ReduceLayerCountByMoment(BattleMomentType.AfterUnderAction);
    }
    protected virtual void OnAfterUnderAction(MomentParamModel paramModel) {}

    public virtual void AfterAction(MomentParamModel paramModel)
    {  
        if (CanTriggerBuffEffect())
        {
            if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
            {
                return;
            }
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.AfterActionMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, paramModel, Model.LayerCount, BattleMomentType.AfterAction));
            }

            OnAfterAction(paramModel);
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
    protected virtual void OnAfterAction(MomentParamModel paramModel) {}

    public void ActionWheelEnd()
    {
        if (CanTriggerBuffEffect())
        {
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.ActionWheelEndMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null,
                    Model.LayerCount, BattleMomentType.ActionWheelEnd));
            }

            OnActionWheelEnd();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.ActionWheelEnd);
    }
    protected virtual void OnActionWheelEnd() {}
    
    public virtual void RoundEnd()
    {  
        if (CanTriggerBuffEffect())
        {
            var subjectID = Model.Subject.EntityID;
            var spellCasterID = Model.SpellCaster?.EntityID ?? 0;
            foreach (var momentID in Model.Config.RoundEndMoment)
            {
                EnqueueViewModel(BattleMomentManager.TriggerMoment(momentID, subjectID, spellCasterID, null, Model.LayerCount, BattleMomentType.RoundEnd));
            }

            OnRoundEnd();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.RoundEnd);
    }
    protected virtual void OnRoundEnd() {}
    
    public void EnqueueViewModel(Queue<BattleMomentViewModel> viewModelQueue)
    {
        while (viewModelQueue.Any())
        {
            var viewModel = viewModelQueue.Dequeue();
            viewModel.BattleSource = BattleSource.Buff;
            viewModel.ConfigID = Model.Config.ID;
            BattleRecordManager.AddBattleMomentViewModel(viewModel);
        }
    }

    /// <summary>
    /// 减少buff持续时间
    /// </summary>
    protected virtual void ReduceLayerCountByMoment(BattleMomentType momentType, MomentParamModel paramModel = null)
    {
        if (!Model.Valid)
        {
            return;
        }
        
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

    protected bool CanTriggerBuffEffect()
    {
        if(!Model.Valid)
        {
            return false;
        }
        
        if (Model.BuffType == BuffType.Gain && Model.Subject.HasBuffMechanism(BuffMechanism.NotBeAddGainBuff))
        {
            return false;
        }

        if (Model.BuffType == BuffType.Abnormal && Model.Subject.HasBuffMechanism(BuffMechanism.NotEffectAbnormalBuff))
        {
            return false;
        }

        return true;
    }
}
