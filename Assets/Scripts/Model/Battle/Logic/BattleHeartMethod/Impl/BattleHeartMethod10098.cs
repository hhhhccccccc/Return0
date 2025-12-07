using System;
using System.Collections.Generic;
using System.Linq;
using cfg;
using Zenject;

public class BattleHeartMethod10098 : BattleHeartMethodBase
{
    public override void RoundEnd()
    {
        base.RoundEnd();
        var buffs = Subject.GetRandomBuffByType(BuffType.Gain);
        foreach (var buff in buffs)
        {
            Subject.ClearBuff(buff.BuffID);
        }
        
        buffs = Subject.GetRandomBuffByType(BuffType.Abnormal);
        foreach (var buff in buffs)
        {
            Subject.ClearBuff(buff.BuffID);
        }
    }
}