using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10151 : BattleHeartMethodBase
{
    public bool InTrigger { get; set; }

    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        InTrigger = false;
    }


    public override void RoundStart()
    {
        base.RoundStart();
        InTrigger = true;
    }

    public override float GetProperty(BattlePropertyType propertyType, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.PowerInt)
        {
            return GetParamFloat(0) + GetParamFloat(1) * Subject.Gr;
        }

        return 0;
    }

    protected override void OnRecycle() 
    {
        InTrigger = false;
    }
}