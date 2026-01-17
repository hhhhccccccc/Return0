using System;
using System.Collections.Generic;
using cfg;

public class MomentViewParamDataModel
{
    public int EntityID { get; set; }
    public Queue<BattleMomentViewModel> SelfActionWheel = new();
    public Queue<BattleMomentViewModel> BeforeAction = new();
    public Queue<BattleMomentViewModel> BeforeUnderAction = new();
    public Queue<BattleMomentViewModel> BeforeClash = new();
    public Queue<BattleMomentViewModel> AfterClash = new();
    public Queue<BattleMomentViewModel> CostResource = new();
    public Queue<BattleMomentViewModel> AfterUnderAction = new();
    public Queue<BattleMomentViewModel> AfterAction = new();
}

public class MomentViewParamModel : IModel, IRecycle
{
    public Queue<BattleMomentViewModel> EveryActionWheel = new();
    public MomentViewParamDataModel SelfViewModel = new();
    public MomentViewParamDataModel OtherViewModel = new();

    public void AddBattleMomentViewModel(BattleMomentViewType momentViewType, BattleMomentViewModel viewModel)
    {
        var entityID = viewModel.EntityID;
        var model = SelfViewModel.EntityID == entityID ? SelfViewModel : OtherViewModel;
        switch (momentViewType)
        {
            case BattleMomentViewType.None:
                break;
            case BattleMomentViewType.BattleStart:
                break;
            case BattleMomentViewType.RoundStart:
                break;
            case BattleMomentViewType.PreDoDesition:
                break;
            case BattleMomentViewType.DoDesition:
                break;
            case BattleMomentViewType.EveryActionWheelStart:
                EveryActionWheel.Enqueue(viewModel);
                break;
            case BattleMomentViewType.SelfActionWheelStart:
                model.SelfActionWheel.Enqueue(viewModel);
                break;
            case BattleMomentViewType.BeforeAction:
                model.BeforeAction.Enqueue(viewModel);
                break;
            case BattleMomentViewType.BeforeUnderAction:
                model.BeforeUnderAction.Enqueue(viewModel);
                break;
            case BattleMomentViewType.BeforeClash:
                model.BeforeClash.Enqueue(viewModel);
                break;
            case BattleMomentViewType.AfterClash:
                model.AfterClash.Enqueue(viewModel);
                break;
            case BattleMomentViewType.CostResource:
                model.CostResource.Enqueue(viewModel);
                break;
            case BattleMomentViewType.AfterUnderAction:
                model.AfterUnderAction.Enqueue(viewModel);
                break;
            case BattleMomentViewType.AfterAction:
                model.AfterAction.Enqueue(viewModel);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(momentViewType), momentViewType, null);
        }
    }

    public Queue<BattleMomentViewModel> GetQueue(int entityID, BattleMomentViewType momentViewType)
    {
        var model = SelfViewModel.EntityID == entityID ? SelfViewModel : OtherViewModel;
        switch (momentViewType)
        {
            case BattleMomentViewType.None:
                break;
            case BattleMomentViewType.BattleStart:
                break;
            case BattleMomentViewType.RoundStart:
                break;
            case BattleMomentViewType.PreDoDesition:
                break;
            case BattleMomentViewType.DoDesition:
                break;
            case BattleMomentViewType.EveryActionWheelStart:
                return EveryActionWheel;
            case BattleMomentViewType.SelfActionWheelStart:
                return model.SelfActionWheel;
            case BattleMomentViewType.BeforeAction:
                return model.BeforeAction;
            case BattleMomentViewType.BeforeUnderAction:
                return model.BeforeUnderAction;
            case BattleMomentViewType.BeforeClash:
                return model.BeforeClash;
            case BattleMomentViewType.AfterClash:
                return model.AfterClash;
            case BattleMomentViewType.CostResource:
                return model.CostResource;
            case BattleMomentViewType.AfterUnderAction:
                 return model.AfterUnderAction;
            case BattleMomentViewType.AfterAction:
                return model.AfterAction;
            default:
                throw new ArgumentOutOfRangeException(nameof(momentViewType), momentViewType, null);
        }

        return null;
    }
    
    public void Recycle()
    {
        EveryActionWheel.Clear();
        SelfViewModel.EntityID = 0;
        SelfViewModel.SelfActionWheel.Clear();
        SelfViewModel.BeforeAction.Clear();
        SelfViewModel.BeforeClash.Clear();
        SelfViewModel.AfterClash.Clear();
        SelfViewModel.AfterAction.Clear();
        
        OtherViewModel.EntityID = 0;
        OtherViewModel.SelfActionWheel.Clear();
        OtherViewModel.BeforeAction.Clear();
        OtherViewModel.BeforeClash.Clear();
        OtherViewModel.AfterClash.Clear();
        OtherViewModel.AfterAction.Clear();
    }
}
