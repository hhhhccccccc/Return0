using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10107 : BattleHeartMethodBase
{
    public override void AfterClash(MomentParamModel paramModel)
    {
        base.AfterClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            if (model.GetSelfClashState(Subject.EntityID))
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
}