using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_ChangeTargetRandom : BattleMomentEffect
{
    protected override void OnEffect()
    {
        var opponentUnitList = BattleManager.GetAllOpponentUnit(Subject.EntityID, true);
        var target = Util.GetRandom(opponentUnitList);
        if (Subject != null && target != null)
        {
            BattleLogicBehaviourManager.ChangeTarget(Subject, target);
        }
    }
}