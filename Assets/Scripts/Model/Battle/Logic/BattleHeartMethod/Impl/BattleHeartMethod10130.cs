using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10130 : BattleHeartMethodBase
{
    public override void AfterUnderAction(MomentParamModel paramModel)
    {
        base.AfterUnderAction(paramModel);
        if (paramModel is DamageParamModel model)
        {
            if (model.BattleClashType == BattleClashType.SingleAction &&
                (model.AttackSkillType == SkillType.PowerKilling || model.AttackSkillType == SkillType.ArtKilling))
            {
                Subject.ChangeProperty(BattlePropertyType.PowerInt, GetParamFloat(0) + GetParamFloat(1) * Subject.Gr);
            }
        }
    }
}