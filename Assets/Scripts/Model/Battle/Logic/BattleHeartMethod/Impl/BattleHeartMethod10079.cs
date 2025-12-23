using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10079 : BattleHeartMethodBase
{
    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var otherHp = model.GetOtherHpValue(Subject.EntityID);
            if (otherHp > 0)
            {
                var otherKeyCost = model.GetOtherKeyCost(Subject.EntityID);
                if (otherKeyCost.Any(key => key.Pollution))
                {
                    Subject.ChangeProperty_Abs(BattlePropertyType.Hp, otherHp);
                }
            }
        }
    }
}