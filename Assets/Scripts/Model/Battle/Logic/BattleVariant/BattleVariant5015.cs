using System;
using System.Collections.Generic;
using cfg;

public class BattleVariant5015 : BattleVariantBase
{
    //随机减少一个键
    public override void AfterAction(MomentParamModel paramModel)
    {
        DoRemoveRandomKey(Subject, 1, ChangeKeyReason.VariantEffect, ChangeKeyType.Remove);
    }

    //与杀式交锋时只会被威力超过了10的百分比的招式破招，随机减少一个键
    public override bool CheckDontBeCounter(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            var other = GetOtherUnit(paramModel);
            if (CheckSkillIsKillingStyle(other, true))
            {
                var selfRate = model.GetSelfFinalWellyRate(Subject.EntityID);
                var otherRate = model.GetOtherFinalWellyRate(Subject.EntityID);
                if (otherRate - selfRate <= 0.1f)
                {
                    return true;
                }
            }
        }

        return false;
    }
}
