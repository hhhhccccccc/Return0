using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10074 : BattleHeartMethodBase
{
    public override bool CanIgnoreSkillDirectDamage(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var targetID = model.GetOtherID(Subject.EntityID);
            var target = BattleManager.GetUnit(targetID);
            if (target != null)
            {
                if (target.RoundAlreadyActionTimes == GetParamInt(0) - 1)
                {
                    return false;
                }
            }
        }

        return true;
    }
}