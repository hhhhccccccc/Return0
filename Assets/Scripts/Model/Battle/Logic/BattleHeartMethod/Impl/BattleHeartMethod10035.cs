using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10035 : BattleHeartMethodBase
{
    public override void AfterClash(MomentParamModel paramModel)
    {
        if (paramModel is DamageParamModel model)
        {
            if (model.CheckClashIsWin(Subject.EntityID))
            {
                DoHealHp(Subject, GetConfigParamFloat(0) + GetConfigParamFloat(1) * Subject.Gr, BattleSource.HeartMethod);
            }
        }
    }
}