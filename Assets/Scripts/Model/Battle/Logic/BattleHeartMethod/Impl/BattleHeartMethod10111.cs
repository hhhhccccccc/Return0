using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using UnityEngine;
using Zenject;

public class BattleHeartMethod10111 : BattleHeartMethodBase
{
    public override void Init(int heartMethodID, BattleUnit subject)
    {
        base.Init(heartMethodID, subject);
        Subject.AddNotRecoverGangQiNatural(1);
        Subject.AddNotRecoverXuanQiNatural(1);
    }

    public override void RoundStart()
    {
        var keyCount = Subject.GetAllKeyCount();
        var maxKeyCount = Subject.GetKeyPropertyMax();
        var delta = maxKeyCount - keyCount;
        if (delta > 0)
        {
            var gangQiPct = GetProperty(BattlePropertyType.GangQi) / GetProperty(BattlePropertyType.MaxGangQi);
            var xuanQiPct = GetProperty(BattlePropertyType.XuanQi) / GetProperty(BattlePropertyType.MaxXuanQi);
            var single = GetParamFloat(0);
            if (gangQiPct >= xuanQiPct)
            {
                var cost = GetProperty(BattlePropertyType.MaxGangQi) * single * delta;
                Subject.ChangeProperty(BattlePropertyType.GangQi, cost, BattleSource.HeartMethod);
            }
            else
            {
                var cost = GetProperty(BattlePropertyType.MaxXuanQi) * single * delta;
                Subject.ChangeProperty(BattlePropertyType.XuanQi, cost, BattleSource.HeartMethod);
            }
        }
    }

    public override void AfterChangeProperty(BattlePropertyType propType, float originPropValue, float finalPropValue,
        BattleSource source = BattleSource.None)
    {
        if (GetProperty(BattlePropertyType.GangQi) <= 0 && GetProperty(BattlePropertyType.XuanQi) <= 0)
        {
            Subject.SetBreak(true);
        }
    }
}