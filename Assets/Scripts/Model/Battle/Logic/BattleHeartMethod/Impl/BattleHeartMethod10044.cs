using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10044 : BattleHeartMethodBase
{
    public override void ReduceHp(float reduceHp, DamageType damageType, int attackID)
    {
        BattleBuffManager.AddBuff(Subject, GameConst.Battle.ShieldBuffID, Subject, (int)(reduceHp * GetParamFloat(0)));
    }
}