using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10108 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        base.RoundStart();
        if (Subject.GetProperty(BattlePropertyType.Hp) / Subject.GetProperty(BattlePropertyType.MaxHp) <=
            GetParamFloat(0))
        {
            var addKeyList = Subject.AddRandomKey(GetParamInt(1), ChangeKeyReason.HeartMethodEffect);
            if (addKeyList is { Count: > 0 })
            {
                var viewModel = AllocViewModel(Subject.EntityID, MomentViewType.AddKey);
                viewModel.AddKeyList(addKeyList);
                EnqueueViewModel(viewModel);
            }
        }
    }
}