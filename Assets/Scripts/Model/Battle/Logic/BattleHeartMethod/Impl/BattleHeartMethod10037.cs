using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10037 : BattleHeartMethodBase
{
    public override void AfterClash(MomentParamModel paramModel)
    {
        base.AfterClash(paramModel);
        if (paramModel is DamageParamModel model)
        {
            if ((model.AttackID == Subject.EntityID && model.AttackClashWin) ||
                (model.HitID == Subject.EntityID && model.HitClashWin))
            {
                Subject.ChangeProperty(BattlePropertyType.XuanQi, GetParamFloat(0), BattleSource.HeartMethod);
            }
        }
    }
}