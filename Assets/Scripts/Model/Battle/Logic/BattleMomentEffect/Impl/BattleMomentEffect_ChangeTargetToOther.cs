using System;
using System.Collections.Generic;
using cfg;
using Zenject;

public class BattleMomentEffect_ChangeTargetToOther : BattleMomentEffect
{
    [Inject] private BattleLogicBehaviourManager BattleLogicBehaviourManager { get; set; }
    protected override void OnEffect()
    {
        var target1 = GetUnitByParamID(Config.ParamList[0]);
        var target2 = GetUnitByParamID(Config.ParamList[1]);
        if (target1 != null && target1.IsAlive() && target2 != null && target2.IsAlive())
        {
            BattleLogicBehaviourManager.ChangeTarget(target1, target2);
        }
    }
}