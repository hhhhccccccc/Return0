using System.Collections.Generic;
using cfg;

public class BattleTreasure10112 : BattleTreasureBase
{
    private Queue<BattleKey> StoreKeyList = new();
    private int Max => GetParamInt(0);
    protected override void OnTryStoreBattleKey(BattleKeyType keyType, ref int count)
    {
        if (StoreKeyList.Count >= Max || count <= 0)
        {
            return;
        }
        var viewModel = AllocViewModel(Subject.EntityID, MomentViewType.StoreKey);
        while (StoreKeyList.Count < Max && count > 0)
        {
            var keyData = PM.GetClass<BattleKey>();
            keyData.AllocGuid();
            keyData.KeyType = keyType;
            keyData.Locked = false;
            keyData.Pollution = false;
            StoreKeyList.Enqueue(keyData);
            count--;
            viewModel.AddKey(keyData);
        }
        EnqueueViewModel(viewModel);
    }

    protected override void OnRoundStart()
    {
        if (Subject.GetAllKeyCount() >= Subject.GetKeyPropertyMax() || StoreKeyList.Count <= 0)
        {
            return;
        }
        var viewModel = AllocViewModel(Subject.EntityID, MomentViewType.ConvertStoreKey);
        while (Subject.GetAllKeyCount() < Subject.GetKeyPropertyMax() && StoreKeyList.Count > 0)
        {
            var key = StoreKeyList.Dequeue();
            var addKeyList = Subject.AddBattleKey(key, ChangeKeyReason.TreasureEffect, ChangeKeyType.Back);
            foreach (var addKey in addKeyList)
            {
                viewModel.AddKey(addKey);
            }
        }
        EnqueueViewModel(viewModel);
    }

    protected override void OnTreasureRecycle()
    {
        foreach (var key in StoreKeyList)
        {
            PM.RecycleClass(key);
        }
        
        StoreKeyList.Clear();
    }
}


