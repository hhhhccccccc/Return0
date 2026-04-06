using System;
using System.Collections.Generic;
using cfg;

public class BattleTreasure10112 : BattleTreasureBase
{
    private Queue<BattleKey> StoreKeyList = new();
    private int Max => GetConfigParamInt(0);
    protected override void OnTryStoreBattleKey(BattleKeyType keyType, ref int count)
    {
        if (StoreKeyList.Count >= Max || count <= 0)
        {
            return;
        }
        while (StoreKeyList.Count < Max && count > 0)
        {
            var keyData = PM.GetClass<BattleKey>();
            keyData.AllocGuid();
            keyData.KeyType = keyType;
            keyData.Locked = false;
            keyData.Pollution = false;
            StoreKeyList.Enqueue(keyData);
            count--;
        }
    }

    protected override void OnRoundStart()
    {
        if (Subject.GetAllKeyCount() >= Subject.GetKeyPropertyMax() || StoreKeyList.Count <= 0)
        {
            return;
        }
        var delta = Subject.GetKeyPropertyMax() - Subject.GetAllKeyCount();
        var min = Math.Min(StoreKeyList.Count, delta);
        var list = new List<BattleKey>();
        for (int i = 0; i < min; i++)
        {
            list.Add(StoreKeyList.Dequeue());
        }
        DoAddKey(Subject, list, ChangeKeyReason.TreasureEffect, ChangeKeyType.Back);
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


