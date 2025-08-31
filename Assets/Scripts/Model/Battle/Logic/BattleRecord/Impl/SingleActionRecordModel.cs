using System;
using System.Collections.Generic;
using cfg;

public class SingleActionRecordModel : BattleRecordModel
{
    public override BattleClashType BattleClashType => BattleClashType.SingleAction;

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
                break;
            case BattleMomentType.AfterClash:
                break;
            case BattleMomentType.ReleaseSkillAction:
                if (entityID == SubjectID)
                {
                    return Subject_ReleaseSkillAction;
                }
                break;
            case BattleMomentType.AfterUnderAction:
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
                break;
            case BattleMomentType.RoundEnd:
                break;
            case BattleMomentType.BuffAdd:
                break;
            case BattleMomentType.BuffEnd:
                break;
            case BattleMomentType.SkillEnd:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(momentType), momentType, null);
        }

        return null;
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
                break;
            case BattleMomentType.AfterClash:
                break;
            case BattleMomentType.ReleaseSkillAction:
                if (viewModel.EntityID == SubjectID)
                {
                    Subject_ReleaseSkillAction.Enqueue(viewModel);
                }
                break;
            case BattleMomentType.AfterUnderAction:
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
                break;
            case BattleMomentType.RoundEnd:
                break;
            case BattleMomentType.BuffAdd:
                break;
            case BattleMomentType.BuffEnd:
                break;
            case BattleMomentType.SkillEnd:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(viewModel.BattleMomentType), viewModel.BattleMomentType, null);
        }
    }
}
