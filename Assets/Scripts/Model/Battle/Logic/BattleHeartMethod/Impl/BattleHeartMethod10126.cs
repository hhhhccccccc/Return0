using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10126 : BattleHeartMethodBase
{
    public override void AfterAction(MomentParamModel paramModel)
    {
        base.AfterAction(paramModel);
        Subject.ChangeProperty(BattlePropertyType.GangQiPct, GetParamFloat(0), BattleSource.HeartMethod);
    }
}