using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10120 : BattleHeartMethodBase
{
    private bool InTrigger { get; set; }

    public override void AfterChangeHp(bool isReduce, float changeHp, DamageType damageType, int attackID, bool isReduceHpMax)
    {
        if (Subject.GetProperty(BattlePropertyType.Hp) / Subject.GetProperty(BattlePropertyType.MaxHp) <=
            GetParamFloat(0))
        {
            if (!InTrigger)
            {
                var buff = Subject.GetBuff(GameConst.Battle.BuffShouHuaShen);
                if (buff == null)
                {
                    buff = BattleBuffManager.AddBuff(Subject, GameConst.Battle.BuffShouHuaShen, Subject, GetParamInt(1));
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
            var buff = Subject.GetBuff(GameConst.Battle.BuffShouHuaShen);
            if (buff != null)
            {
                buff.AddBuffNotLowerLayerCount(false, GetParamInt(1));
                InTrigger = false;
            }
        }
    }
}