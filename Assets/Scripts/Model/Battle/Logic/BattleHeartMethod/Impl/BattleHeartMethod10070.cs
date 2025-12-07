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
            if ((model.AttackID == Subject.EntityID && model.AttackClashWin)
                || (model.HitID == Subject.EntityID && model.HitClashWin))
            {
                var damageDelta = Math.Abs(model.AttackFinalDamageRate - model.HitFinalDamageRate);
                Subject.ChangeProperty(BattlePropertyType.GangQi, damageDelta / GetParamFloat(0),
                    BattleSource.HeartMethod);
            }
        }
    }
}