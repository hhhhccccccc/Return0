using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

//todo 表现
public class BattleHeartMethod10058 : BattleHeartMethodBase
{
    public override void RoundStart()
    {
        base.RoundStart();
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffNiSha, Subject, GetConfigParamInt(0));
    }
}