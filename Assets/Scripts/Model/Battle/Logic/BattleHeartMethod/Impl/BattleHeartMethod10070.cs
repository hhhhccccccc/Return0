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
                var damageDelta = Math.Abs(model.GetSelfFinalDamageWelly(Subject.EntityID) - model.GetOtherFinalDamageWelly(Subject.EntityID));
                Subject.ChangeProperty(BattlePropertyType.GangQi, damageDelta / GetParamFloat(0),
                    BattleSource.HeartMethod);
            }
        }
    }
}