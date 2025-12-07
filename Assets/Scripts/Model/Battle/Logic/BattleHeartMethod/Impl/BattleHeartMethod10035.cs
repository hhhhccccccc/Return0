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
            if ((model.AttackID == Subject.EntityID && model.AttackClashWin) ||
                (model.HitID == Subject.EntityID && model.HitClashWin))
            {
                Subject.HealHp(GetParamFloat(0) + GetParamFloat(1) * Subject.Gr, BattleSource.HeartMethod);
            }
        }
    }
}