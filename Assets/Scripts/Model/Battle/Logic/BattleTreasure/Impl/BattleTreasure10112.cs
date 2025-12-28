using System.Collections.Generic;
using cfg;

public class BattleTreasure10112 : BattleTreasureBase
{
    private Queue<BattleKey> StoreKeyList = new();
    private int Max => GetParamInt(0);
    protected override void OnTryStoreBattleKey(BattleKeyType keyType, ref int count)
    {
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
        while (Subject.GetAllKeyCount() < Subject.GetKeyPropertyMax() && StoreKeyList.Count > 0)
        {
            var key = StoreKeyList.Dequeue();
            var addKeyList = Subject.AddBattleKey(key, ChangeKeyReason.TreasureEffect);
        }
    }

    protected override void OnRecycle()
    {
        foreach (var key in StoreKeyList)
        {
            PM.RecycleClass(key);
        }
        
        StoreKeyList.Clear();
    }
}


