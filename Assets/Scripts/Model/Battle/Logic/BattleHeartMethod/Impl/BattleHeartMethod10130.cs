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
        if (paramModel is DamageParamModel model)
        {
            if (model.BattleClashType == BattleClashType.SingleAction &&
                (model.GetOtherSkillType(Subject.EntityID) == SkillType.PowerKilling || model.GetOtherSkillType(Subject.EntityID) == SkillType.ArtKilling))
            {
                DoChangeProperty(Subject, BattlePropertyType.PowerInt, GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr, BattleSource.HeartMethod);
            }
        }
    }
}