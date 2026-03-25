using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10104 : BattleHeartMethodBase
{
    public override void KeyReduce(BattleKeyType keyType, List<BattleKey> changeKeyData, ChangeKeyReason reason, ChangeKeyType changeType)
    {
        if (Subject.GetAllKeyCount() <= 0)
        {
            var addList = Subject.AddRandomKey(GetParamInt(0), ChangeKeyReason.HeartMethodEffect);
            if (addList is { Count: > 0 })
            {
                var viewModel = AllocViewModel(Subject.EntityID, MomentViewType.AddKey);
                viewModel.AddKeyList(addList);
                EnqueueViewModel(viewModel);
            }
        }
    }
}