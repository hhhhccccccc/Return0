using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10140 : BattleHeartMethodBase
{
    public override void BattleStart()
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10121, Subject, GetParamInt(0));
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff10131, Subject, GetParamInt(1));
    }
}