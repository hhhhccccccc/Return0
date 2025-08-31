using System;
using System.Collections.Generic;
using System.Reflection;
using Zenject;

public class BattleStartRecordController : ControllerBase<BattleStartActEventModel>
{
    private static Dictionary<string, Type> TypeDic = new();
    private static Assembly ViewAssembly = GameUtil.GetAssembly("View");
    [Inject] private BattleRecordManager BattleRecordManager;
    [Inject] private BattleLogicStateManager BattleLogicStateManager;
    [Inject] private IJobManager JobManager;
    [Inject] private IPoolManager PoolManager;

    private Queue<BattleRecordModel> RecordModelQueue;
    
    public override void Handle(BattleStartActEventModel model)
    {
        RecordModelQueue = BattleRecordManager.GetRecordModelQueue();
        Act();
    }

    private Type GetTypeByName(string typeName)
    {
        if (!TypeDic.TryGetValue(typeName, out var type))
        {
            type = ViewAssembly.GetType(typeName);
            TypeDic.Add(typeName, type);
        }

        return type;
    }
    
    private void Act()
    {
        if (RecordModelQueue.Count > 0)
        {
            var recordModel = RecordModelQueue.Dequeue();
            var recordType = recordModel.BattleClashType;
            var typeName = $"{recordType}RecordViewHandleModel";
            var type = GetTypeByName(typeName);
            var instance = PoolManager.GetClass(type);
            var handle = (IRecordViewHandleModel)instance;
            JobManager.AddJob(JobPriority.Low, handle.Handle(recordModel, Act));
        }
        else
        {
            BattleLogicStateManager.TryEnd();
        }
    }
}
