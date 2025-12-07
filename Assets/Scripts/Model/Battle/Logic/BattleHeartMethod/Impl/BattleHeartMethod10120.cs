using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10120 : BattleHeartMethodBase
{
    private bool InTrigger { get; set; }
    public override void HpChanged()
    {
        base.HpChanged();
        if (Subject.GetProperty(BattlePropertyType.Hp) / Subject.GetProperty(BattlePropertyType.MaxHp) <=
            GetParamFloat(0))
        {
            if (!InTrigger)
            {
                var buff = Subject.GetBuff(GameConst.Battle.Buff30371);
                if (buff == null)
                {
                    buff = BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff30371, Subject, GetParamInt(1));
                }

                if (buff != null)
                {
                    buff.AddBuffNotLowerLayerCount(true, GetParamInt(1));
                }

                InTrigger = true;
            }
        }
        else if (InTrigger)
        {
            var buff = Subject.GetBuff(GameConst.Battle.Buff30371);
            if (buff != null)
            {
                buff.AddBuffNotLowerLayerCount(false, GetParamInt(1));
                InTrigger = false;
            }
        }
    }

    public override void Recycle()
    {
        InTrigger = false;
        base.Recycle();
    }
}