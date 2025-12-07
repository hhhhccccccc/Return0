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
            OnBattleStart();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.BattleStart);
    }
    protected virtual void OnBattleStart() {}

    public virtual void RoundStart()
    {
        if (CanTriggerBuffEffect())
        {
            OnRoundStart();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.RoundStart);
    }
    protected virtual void OnRoundStart() {}
    
    public void CalculateActionWheel()
    {
        if (CanTriggerBuffEffect())
        {
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
            OnDoDesitionAction();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.DoDesitionAction);
    }
    protected virtual void OnDoDesitionAction() {}
    
    public void EveryActionWheelStart()
    {
        if (CanTriggerBuffEffect())
        {
            OnEveryActionWheelStart();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.EveryActionWheelStart);
    }
    protected virtual void OnEveryActionWheelStart() {}

    public virtual void SelfActionWheelStart()
    {
        if (CanTriggerBuffEffect())
        {
            OnSelfActionWheelStart();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.ActionWheelStart);
    }
    protected virtual void OnSelfActionWheelStart() {}

    public virtual void BeforeAction()
    {
        if (CanTriggerBuffEffect())
        {
            if (Model.Subject.NotBeAbnormalBuffEffect > 0 && Model.BuffType == BuffType.Abnormal)
            {
                return;
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
            OnActionWheelEnd();
        }
        
        ReduceLayerCountByMoment(BattleMomentType.ActionWheelEnd);
    }
    protected virtual void OnActionWheelEnd() {}
    
    public virtual void RoundEnd()
    {  
        if (CanTriggerBuffEffect())
        {
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
