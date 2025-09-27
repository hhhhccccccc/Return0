using System;
using System.Collections.Generic;
using cfg;

public class DoubleClashRecordModel : BattleRecordModel
{
    public override BattleClashType BattleClashType => BattleClashType.DoubleClash;

    private Queue<BattleMomentViewModel> Subject_BeforeClash { get; set; } = new();
    private Queue<BattleMomentViewModel> Target_BeforeClash { get; set; } = new();
    
    public bool CheckSubjectCostInClash { get; set; }
    public bool CheckTargetCostInClash { get; set; }
    
    //交锋时的威力对比
    private float Subject_InClashSkillDamageRate { get; set; }
    private float Target_InClashSkillDamageRate { get; set; }

    private Queue<BattleMomentViewModel> Subject_AfterClash { get; set; } = new();
    private Queue<BattleMomentViewModel> Target_AfterClash { get; set; } = new();
    
    private Queue<BattleMomentViewModel> Subject_AfterUnderAction { get; set; }= new();
    private Queue<BattleMomentViewModel> Target_AfterAction { get; set; } = new();
    
    public override void Recycle()
    {
        base.Recycle();
        CheckSubjectCostInClash = false;
        CheckTargetCostInClash = false;
        
        Subject_InClashSkillDamageRate = 0;
        Target_InClashSkillDamageRate = 0;
        
        foreach (var viewModel in Subject_BeforeClash)
        {
            PoolManager.RecycleClass(viewModel);
        }
        Subject_BeforeClash.Clear();
        
        foreach (var viewModel in Target_BeforeClash)
        {
            PoolManager.RecycleClass(viewModel);
        }
        Target_BeforeClash.Clear();
        
        foreach (var viewModel in Subject_AfterClash)
        {
            PoolManager.RecycleClass(viewModel);
        }
        Subject_AfterClash.Clear();
        
        foreach (var viewModel in Target_AfterClash)
        {
            PoolManager.RecycleClass(viewModel);
        }
        Target_AfterClash.Clear();
    }
    
    public void SetInClashSkillDamageRate(int entityID, float damageRate)
    {
        if (SubjectID == entityID)
        {
            Subject_InClashSkillDamageRate = damageRate;
        }

        if (TargetID == entityID)
        {
            Target_InClashSkillDamageRate = damageRate;
        }
    }
    
    public float GetInClashSkillDamageRate(int entityID)
    {
        if (SubjectID == entityID)
        {
            return Subject_InClashSkillDamageRate;
        }

        if (TargetID == entityID)
        {
            return Target_InClashSkillDamageRate;
        }

        return 0;
    }

    public override void AddBattleMomentViewModel(BattleMomentViewModel viewModel)
    {
        switch (viewModel.BattleMomentType)
        {
            case BattleMomentType.BattleStart:
                break;
            case BattleMomentType.RoundStart:
                break;
            case BattleMomentType.DoDesitionAction:
                break;
            case BattleMomentType.BeforeAction:
                if (viewModel.EntityID == SubjectID)
                {
                    Subject_BeforeAction.Enqueue(viewModel);
                }
                break;
            case BattleMomentType.BeforeUnderAction:
                if (viewModel.EntityID == TargetID)
                {
                    Target_BeforeUnderAction.Enqueue(viewModel);
                }
                break;
            case BattleMomentType.BeforeClash:
                if (viewModel.EntityID == SubjectID)
                {
                    Subject_BeforeClash.Enqueue(viewModel);
                }
                if (viewModel.EntityID == TargetID)
                {
                    Target_BeforeClash.Enqueue(viewModel);
                }
                break;
            case BattleMomentType.AfterClash:
                if (viewModel.EntityID == SubjectID)
                {
                    Subject_AfterClash.Enqueue(viewModel);
                }
                if (viewModel.EntityID == TargetID)
                {
                    Target_AfterClash.Enqueue(viewModel);
                }
                break;
            case BattleMomentType.ReleaseSkillAction:
                if (viewModel.EntityID == SubjectID)
                {
                    Subject_ReleaseSkillAction.Enqueue(viewModel);
                }
                if (viewModel.EntityID  == TargetID)
                {
                    Target_ReleaseSkillAction.Enqueue(viewModel);
                }
                break;
            case BattleMomentType.AfterUnderAction:
                if (viewModel.EntityID == SubjectID)
                {
                    Subject_AfterUnderAction.Enqueue(viewModel);
                }
                if (viewModel.EntityID == TargetID)
                {
                    Target_AfterUnderAction.Enqueue(viewModel);
                }
                break;
            case BattleMomentType.AfterAction:
                if (viewModel.EntityID == SubjectID)
                {
                    Subject_AfterAction.Enqueue(viewModel);
                }
                if (viewModel.EntityID == TargetID)
                {
                    Target_AfterAction.Enqueue(viewModel);
                }
                break;
            case BattleMomentType.RoundEnd:
                break;
            case BattleMomentType.BuffAddLayer:
                break;
            case BattleMomentType.BuffReduceLayer:
                break;
            case BattleMomentType.SkillEnd:
                break;
            case BattleMomentType.BuffRemove:
                break;
            case BattleMomentType.ActionWheelStart:
                break;
            case BattleMomentType.CalculateActionWheel:
                break;
            case BattleMomentType.BuffStart:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(viewModel.BattleMomentType), viewModel.BattleMomentType, null);
        }
    }

    public override Queue<BattleMomentViewModel> GetQueue(BattleMomentType momentType, int entityID)
    {
        switch (momentType)
        {
            case BattleMomentType.BattleStart:
                break;
            case BattleMomentType.RoundStart:
                break;
            case BattleMomentType.DoDesitionAction:
                break;
            case BattleMomentType.BeforeAction:
                if (entityID == SubjectID)
                {
                    return Subject_BeforeAction;
                }
                break;
            case BattleMomentType.BeforeUnderAction:
                if (entityID == TargetID)
                {
                    return Target_BeforeUnderAction;
                }
                break;
            case BattleMomentType.BeforeClash:
                if (entityID == SubjectID)
                {
                    return Subject_BeforeClash;
                }
                if (entityID == TargetID)
                {
                    return Target_BeforeClash;
                }
                break;
            case BattleMomentType.AfterClash:
                if (entityID == SubjectID)
                {
                    return Subject_AfterClash;
                }
                if (entityID == TargetID)
                {
                    return Target_AfterClash;
                }
                break;
            case BattleMomentType.ReleaseSkillAction:
                if (entityID == SubjectID)
                {
                    return Subject_ReleaseSkillAction;
                }
                if (entityID == TargetID)
                {
                    return Target_ReleaseSkillAction;
                }
                break;
            case BattleMomentType.AfterUnderAction:
                if (entityID == SubjectID)
                {
                    return Subject_AfterUnderAction;
                }
                if (entityID == TargetID)
                {
                    return Target_AfterUnderAction;
                }
                break;
            case BattleMomentType.AfterAction:
                if (entityID == SubjectID)
                {
                    return Subject_AfterAction;
                }
                if (entityID == TargetID)
                {
                    return Target_AfterAction;
                }
                break;
            case BattleMomentType.RoundEnd:
                break;
            case BattleMomentType.BuffAddLayer:
                break;
            case BattleMomentType.BuffReduceLayer:
                break;
            case BattleMomentType.SkillEnd:
                break;
            case BattleMomentType.BuffRemove:
                break;
            case BattleMomentType.ActionWheelStart:
                break;
            case BattleMomentType.CalculateActionWheel:
                break;
            case BattleMomentType.BuffStart:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(momentType), momentType, null);
        }

        return null;
    }
}
