using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10035 : BattleHeartMethodBase
{
    public override void AfterClash(MomentParamModel paramModel)
    {
        base.AfterClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            if (model.CheckClashIsWin(Subject.EntityID))
            {
                var finalValue = Subject.HealHp(GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr, BattleSource.HeartMethod);
                EnqueueViewModel(Subject.EntityID, MomentViewType.ChangeHp, finalValue);
            }
        }
    }
}