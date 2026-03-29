using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10150 : BattleHeartMethodBase
{
    private bool IsGangQi { get; set; }
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        IsGangQi = true;
    }

    public override void AfterGetProperty(BattlePropertyType propertyType, ref float value, GetPropertySourceModel model = null)
    {
        if (propertyType == BattlePropertyType.GangQiRedInt && IsGangQi && value <= GetParamFloat(0))
        {
            value = GetParamFloat(0);
        }
        
        if (propertyType == BattlePropertyType.XuanQiRedInt && !IsGangQi && value <= GetParamFloat(0))
        {
            value = GetParamFloat(0);
        }
    }

    public override void RoundStart()
    {
        base.RoundStart();
        IsGangQi = !IsGangQi;
    }

    protected override void OnHeartMethodRecycle()
    {
        IsGangQi = false;
    }
}