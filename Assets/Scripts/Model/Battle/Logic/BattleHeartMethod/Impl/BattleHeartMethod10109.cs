using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10109 : BattleHeartMethodBase
{
    public override void HpChanged()
    {
        if (Subject.RoundBeDirectDamageTimes == 1)
        {
            Subject.AddRandomKey(GetParamInt(0), ChangeKeyReason.HeartMethodEffect);
        }
    }
}