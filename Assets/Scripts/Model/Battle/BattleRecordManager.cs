
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleRecordManager : SingleModel
{
    #region Inject注入

    [Inject] private IPoolManager PoolManager;

    #endregion
    
    #region 表现指令

    private Queue<BattleRecordModel> RecordModelQueue = new();
    
    public Queue<BattleRecordModel> GetRecordModelQueue() => RecordModelQueue;

    private void AddRecordModel(BattleRecordModel model)
    {
        RecordModelQueue.Enqueue(model);
    }

    #endregion

    #region 指令方法
    
    public void AddBattleRecordModel(BattleRecordModel battleRecordModel)
    {
        AddRecordModel(battleRecordModel);
    }
    
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

    private BattleRecordModel CurrentRecordModel;
    private Queue<BattleMomentViewModel> BeforeActionViewModel = new();

    public void AddBattleMomentViewModel(BattleMomentViewModel model)
    {
        if (model.BattleMomentType == BattleMomentType.BeforeAction || model.BattleMomentType == BattleMomentType.BeforeUnderAction)
        {
            BeforeActionViewModel.Enqueue(model);
        }
        else if (CurrentRecordModel != null)
        {
            CurrentRecordModel.AddBattleMomentViewModel(model);
        }
    }

    /// <summary>
    /// 设置之后要操作的记录 并且之后加表现都往这里面加
    /// </summary>
    /// <param name="recordModel"></param>
    public void SetCurrentAndCacheRecordModel(BattleRecordModel recordModel)
    {
        CurrentRecordModel = recordModel;
        while (BeforeActionViewModel.Any())
        {
            var viewModel = BeforeActionViewModel.Dequeue();
            CurrentRecordModel.AddBattleMomentViewModel(viewModel);
        }
    }

    #endregion
}
