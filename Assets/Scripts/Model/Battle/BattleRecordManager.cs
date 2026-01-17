
using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleRecordManager : SingleModel
{
    #region Inject注入

    [Inject] private IPoolManager PoolManager { get; set; }
    [Inject] private BattleManager BattleManager { get; set; }

    #endregion
    
    #region 表现指令

    private Queue<BattleRecordModel> RecordModelQueue = new();
    
    public Queue<BattleRecordModel> GetRecordModelQueue() => RecordModelQueue;
    

    #endregion
    
    public void RoundStart()
    {
        RecordModelQueue.Clear();
    }

    public void RoundEnd()
    {
        RecordModelQueue.Clear();
    }

    #region 中途存一下表现
    private BattleRecordModel CurrentRecordModel { get; set; }
    public void AddBattleMomentViewModel(BattleMomentViewModel model)
    {
        var unit = BattleManager.GetUnit(model.EntityID);
        var viewType = unit.ViewType;
        switch (viewType)
        {
            case BattleMomentViewType.None:
            case BattleMomentViewType.BattleStart:
            case BattleMomentViewType.RoundStart:
            case BattleMomentViewType.PreDoDesition:
            case BattleMomentViewType.DoDesition:
                break;
            case BattleMomentViewType.EveryActionWheelStart:
            case BattleMomentViewType.SelfActionWheelStart:
            case BattleMomentViewType.BeforeAction:
            case BattleMomentViewType.BeforeClash:
            case BattleMomentViewType.AfterClash:
            case BattleMomentViewType.AfterAction:
            case BattleMomentViewType.BeforeUnderAction:
            case BattleMomentViewType.CostResource:
            case BattleMomentViewType.AfterUnderAction:
                if (CurrentRecordModel != null)
                {
                    CurrentRecordModel.AddBattleMomentViewModel(viewType, model);
                }
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    /// <summary>
    /// 设置之后要操作的记录 并且之后加表现都往这里面加
    /// </summary>
    /// <param name="clashType"></param>
    /// <param name="selfID"></param>
    /// <param name="otherID"></param>
    public BattleRecordModel NewRecordModel(BattleClashType clashType, int selfID, int otherID)
    {
        BattleRecordModel model;
        if (clashType == BattleClashType.SingleAction)
        {
            model = PoolManager.GetClass<SingleActionRecordModel>();
        }
        else if (clashType == BattleClashType.SingleClash)
        {
            model = PoolManager.GetClass<SingleClashRecordModel>();
        }
        else
        {
            model = PoolManager.GetClass<DoubleClashRecordModel>();
        }
        
        model.Init(clashType, selfID, otherID);
        RecordModelQueue.Enqueue(model);
        CurrentRecordModel = model;
        return model;
    }
    
    #endregion
}
