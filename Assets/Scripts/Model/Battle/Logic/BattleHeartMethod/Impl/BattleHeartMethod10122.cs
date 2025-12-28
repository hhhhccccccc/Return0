using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10122 : BattleHeartMethodBase
{
    private bool InTrigger { get; set; }
    private int DieCount { get; set; }
    
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        Register<UnitDieEventModel>(OnUnitDie);
        base.Init(heartMethodID, subject);
    }

    private void OnUnitDie(UnitDieEventModel model)
    {
        DieCount++;
        if (!InTrigger && DieCount >= GetParamInt(0))
        {
            var buff = Subject.GetBuff(GameConst.Battle.Buff30391);
            if (buff == null)
            {
                buff = BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff30391, Subject, GetParamInt(1));
            }

            if (buff != null)
            {
                buff.AddBuffNotLowerLayerCount(true, GetParamInt(1));
            }

            InTrigger = true;
        }
    }

     protected override void OnRecycle()
    {
        InTrigger = false;
        DieCount = 0;
    }
}