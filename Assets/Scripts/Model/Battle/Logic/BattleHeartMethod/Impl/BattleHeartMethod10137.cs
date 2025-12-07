using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10137 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10191, Subject, GetParamInt(0));
    }
}