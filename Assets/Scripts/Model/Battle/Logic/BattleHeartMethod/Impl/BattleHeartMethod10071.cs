using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10071 : BattleHeartMethodBase
{
    public bool CanTrigger { get; set; }

    public override void AfterClash(MomentParamModel paramModel)
    {
        base.AfterClash(paramModel);
        if (!CanTrigger)
        {
            return;
        }
        if (paramModel is DamageParamModel model)
        {
            if (model.GetSelfClashState(Subject.EntityID))
            {
                CanTrigger = true;
            }
        }
    }

    public override void BeforeChangeProperty(BattlePropertyType pType, ref float value, BattleSource source)
    {
        if (pType == BattlePropertyType.GangQi || pType == BattlePropertyType.XuanQi)
        {
            value += Util.GetRandomInt(GetParamInt(0), GetParamInt(1));
            CanTrigger = false;
        }
    }

    protected override void OnRecycle()
    {
        CanTrigger = false;
    }
}