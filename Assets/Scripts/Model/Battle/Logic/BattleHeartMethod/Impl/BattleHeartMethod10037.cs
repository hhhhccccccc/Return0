using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10037 : BattleHeartMethodBase
{
    public override void AfterClash(MomentParamModel paramModel)
    {
        base.AfterClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            if (model.CheckClashIsWin(Subject.EntityID))
            {
                var finalValue = Subject.ChangeProperty(BattlePropertyType.XuanQi, GetConfigParamFloat(0), BattleSource.HeartMethod);
                EnqueueViewModel(Subject.EntityID, MomentViewType.ChangeXuanQi, finalValue);
            }
        }
    }
}