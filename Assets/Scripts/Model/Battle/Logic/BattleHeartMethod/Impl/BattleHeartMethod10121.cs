using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10121 : BattleHeartMethodBase
{
    private bool InTrigger { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        InTrigger = false;
    }

    public override void RoundEnd()
    {
        base.RoundEnd();
        if (!InTrigger && BattleLogicStateManager.Round >= GetConfigParamInt(0))
        {
            var buff = Subject.GetBuff(GameConst.Battle.BuffQinHuaShen);
            if (buff == null)
            {
                buff = BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffQinHuaShen, Subject, GetConfigParamInt(1));
            }

            if (buff != null)
            {
                buff.AddBuffNotLowerLayerCount(true, GetConfigParamInt(1));
            }

            InTrigger = true;
        }
    }

    protected override void OnHeartMethodRecycle()
    {
        InTrigger = false;
    }
}