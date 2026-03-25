using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10109 : BattleHeartMethodBase
{
    public override void HpChanged()
    {
        if (Subject.RoundBeDirectDamageTimes == 1)
        {
            var addKeyList = Subject.AddRandomKey(GetParamInt(0), ChangeKeyReason.HeartMethodEffect);
            if (addKeyList is { Count: > 0 })
            {
                var viewModel = AllocViewModel(Subject.EntityID, MomentViewType.AddKey);
                viewModel.AddKeyList(addKeyList);
                EnqueueViewModel(viewModel);
            }
        }
    }
}