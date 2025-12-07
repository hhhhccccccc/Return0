using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10058 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        base.RoundStart();
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff30031, Subject, GetParamInt(0));
    }
}