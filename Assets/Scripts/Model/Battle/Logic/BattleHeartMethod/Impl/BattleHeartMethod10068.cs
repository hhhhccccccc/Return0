using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10068 : BattleHeartMethodBase
{
    public override void BattleStart()
    {
        base.BattleStart();
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff30071, Subject, GetParamInt(0));
    }
}