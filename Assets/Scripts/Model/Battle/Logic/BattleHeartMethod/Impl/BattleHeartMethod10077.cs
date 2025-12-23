using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10077 : BattleHeartMethodBase
{
    public override void BattleStart()
    {
        base.BattleStart();
        var oppoList = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
        foreach (var unit in oppoList)
        {
            BattleBuffManager.AddBuff(unit, GameConst.Battle.Buff90019, Subject, 1);
        }
    }
}