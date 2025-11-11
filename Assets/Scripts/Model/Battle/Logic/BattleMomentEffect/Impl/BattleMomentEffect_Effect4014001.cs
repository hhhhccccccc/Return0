using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_Effect4014001 : BattleMomentEffect
{
    [Inject] private BattleBuffManager BattleBuffManager { get; set; }
    protected override void OnEffect()
    {
        var buff = BattleBuffManager.AddBuff(Subject, GameConst.Battle.Buff30091, Subject, 1);
        if (buff != null)
        {
            buff.SetTarget(Target);
        }
    }
}