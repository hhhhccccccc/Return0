using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10113 : BattleHeartMethodBase
{
    public override void EveryActionWheelStart()
    {
        Subject.ChangeProperty(BattlePropertyType.GangQi, GetParamFloat(0), BattleSource.HeartMethod);
        Subject.ChangeProperty(BattlePropertyType.XuanQi, GetParamFloat(1), BattleSource.HeartMethod);
        Subject.AddRandomKey(GetParamInt(2), ChangeKeyReason.HeartMethodEffect);
    }
}