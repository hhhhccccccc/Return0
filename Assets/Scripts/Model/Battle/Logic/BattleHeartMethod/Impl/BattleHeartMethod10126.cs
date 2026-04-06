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
        DoChangeProperty(Subject, BattlePropertyType.GangQiPct, GetConfigParamFloat(0), BattleSource.HeartMethod);
    }
}