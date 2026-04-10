using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10070 : BattleHeartMethodBase
{
    public override void AfterClash(MomentParamModel paramModel)
    {
        base.AfterClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            if (model.GetSelfClashState(Subject.EntityID))
            {
                var damageDelta = Math.Abs(model.GetSelfFinalWellyRate(Subject.EntityID) - model.GetOtherFinalWellyRate(Subject.EntityID));
                DoChangeProperty(Subject, BattlePropertyType.GangQi, damageDelta / GetConfigParamFloat(0), BattleSource.HeartMethod);
            }
        }
    }
}