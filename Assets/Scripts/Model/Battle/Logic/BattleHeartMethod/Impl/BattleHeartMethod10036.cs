using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10036 : BattleHeartMethodBase
{
    public override void AfterClash(MomentParamModel paramModel)
    {
        base.AfterClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            if (model.CheckClashIsWin(Subject.EntityID))
            {
                var finalValue = Subject.ChangeProperty(BattlePropertyType.GangQi, GetConfigParamFloat(0), BattleSource.HeartMethod);
                EnqueueViewModel(Subject.EntityID, MomentViewType.ChangeGangQi, finalValue);
            }
        }
    }
}